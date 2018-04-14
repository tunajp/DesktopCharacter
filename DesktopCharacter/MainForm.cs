using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DxLibDLL;
using SQLite;

namespace DesktopCharacter
{
    // デスクトップマスコットの作り方
    // https://qiita.com/massoumen/items/2985a0fb30472b97a590

    public partial class MainForm : Form
    {
        /// <summary>
        /// モデルデータ
        /// </summary>
        private List<ModelHandle> modelHandles;

        private SQLiteConnection db;
        public Data.OptionData optionData;
        public Data.ModelData modelData;
        public Data.MotionData motionData;

        public float playSpeed { get; set; }

        float targetX = 0.0f, targetY = 10.0f, targetZ = 0.0f;
        float cameraX = 0.0f, cameraY = 10.0f, cameraZ = -30.0f;
        float ROTATE_SPEED = (float)Math.PI / 90;//回転スピード
        private int XBuf, YBuf;
        public float scale { get; set; }

        public NotifyIcon icon;
        public bool ignoreKeyAndMouse { get; set; }

        public string currentLanguage = "Default";
        public Screen screen { get; set; }
        public string directory { get; }
        public string temp_root_directory;

        public MainForm()
        {
            InitializeComponent();

            //Guid guid = Guid.NewGuid();
            //System.Diagnostics.Trace.WriteLine(guid.ToString());
            // find execute file path
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            this.directory = System.IO.Path.GetDirectoryName(appPath);

            string databaseFile = System.IO.Path.Combine(this.directory, "data", "Data.db");
            db = new SQLiteConnection(databaseFile);

            optionData = new Data.OptionData(db);
            modelData = new Data.ModelData(db);
            motionData = new Data.MotionData(db);

            this.currentLanguage = optionData.getLanguage();

            // switch Localization
            string lang = this.currentLanguage;
            if (lang == "Default")
            {
                lang = "en-US";
            }
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);

            ignoreKeyAndMouse = false;

            Screen[] allscreen = Screen.AllScreens;
            screen = allscreen[0]; // マルチディスプレイの場合はあ0意外でメインディスプレイに投影できる

            // 画面サイズをフォームのサイズに適用する
            ClientSize = new Size(screen.Bounds.Width, screen.Bounds.Height);
            Text = "DesktopCharacter"; // ウィンドウの名前を設定
            this.TopMost = true; // 常に最前面
            ShowInTaskbar = false; // タスクバーに表示させない

            DX.SetOutApplicationLogValidFlag(DX.FALSE); // Log.txtを生成しない
            DX.SetUserWindow(Handle); // DxLibの親ウィンドウをこのフォームに設定
            DX.SetZBufferBitDepth(24); // http://dxlib.o.oo7.jp/cgi/patiobbs/patio.cgi?mode=view&no=3751
            int ret = DX.DxLib_Init(); // DxLibの初期化処理
            if (ret == -1)
            {
                MessageBox.Show(Properties.Resources.InitDxLibFailed, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception();
            }
            DX.SetDrawScreen(DX.DX_SCREEN_BACK); // 描画先を裏画面に設定

            this.scale = 1.0f;

            // モデルファイルとモーションファイルを一時ディレクトリにコピーしてロードする
            this.temp_root_directory = "";
            modelHandles = new List<ModelHandle>();
            loadFiles();

            playSpeed = 0.2f; // モーションの再生位置を進める速度

            // カメラのセットアップ
            DX.SetCameraNearFar(0.1f, 1000.0f); // 奥行0.1-1000をカメラの描画範囲とする
            DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(cameraX, cameraY, cameraZ), DX.VGet(targetX, targetY, targetZ)); // 第1引数の位置から第2引数の位置を見る角度にカメラを設置

            XBuf = 0;
            YBuf = 0;

            TaskTray();
            this.Location = screen.Bounds.Location;
        }

        public void TaskTray()
        {
            // タスクトレイにアイコンを表示する
            icon = new NotifyIcon();
            icon.Icon = new Icon("if_girl-avatar_2903195.ico");
            icon.Visible = true;
            icon.Text = "DesktopCharacter";
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem menuItem0 = new ToolStripMenuItem();
            menuItem0.Text = Properties.Resources.Option;
            menuItem0.Click += new EventHandler(Option_Click);
            menu.Items.Add(menuItem0);

            ToolStripMenuItem menuItem1 = new ToolStripMenuItem();
            menuItem1.Text = Properties.Resources.Exit;
            menuItem1.Click += new EventHandler(Close_Click);
            menu.Items.Add(menuItem1);

            icon.ContextMenuStrip = menu;
        }

        private void Option_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm();
            optionForm.parentForm(this);
            optionForm.Show();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void MainLoop()
        {
            // 毎フレーム呼ぶ処理

            DX.ClearDrawScreen(); // 裏画面を消す
            DX.DrawBox(0, 0, screen.Bounds.Width, screen.Bounds.Height, DX.GetColor(1, 1, 1), DX.TRUE); // 背景を設定(透過させる)

            foreach(ModelHandle mh in this.modelHandles)
            {
                mh.playTime += playSpeed; // 時間を進める
                                       // モーションの再生位置が終端まで来たら最初に戻す
                if (mh.playTime >= mh.totalTime)
                {
                    mh.playTime = 0.0f;
                }
                // モーションの再生位置を設定
                DX.MV1SetAttachAnimTime(mh.modelHandle, mh.attachIndex, mh.playTime);

                DX.MV1DrawModel(mh.modelHandle); // 3Dモデルの描画
            }

            keyAndMouse();

            DX.ScreenFlip(); // 裏画面を表画面にコピー
        }

        private void keyAndMouse()
        {
            if (ignoreKeyAndMouse)
            {
                return;
            }

            // ESCキーを押したら終了
            if (DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) != 0)
            {
                Close();
                return;
            }
            // マウスの左ボタンがクリックされているか
            if ((DX.GetMouseInput() & DX.MOUSE_INPUT_LEFT) != 0)
            {
                int _XBuf;
                int _YBuf;
                if (0 == DX.GetMousePoint(out _XBuf, out _YBuf))
                {
                    // なんか移動がおかしいのはマウスのXY移動とモデルのXY移動が連動しているため、回転してる状態だと移動場所がおかしな感じになるせい
                    // マウスの衝突判定
                    // https://dixq.net/forum/viewtopic.php?t=12473
                    foreach (ModelHandle mh in modelHandles)
                    {
                        // FIXME:モデルと線分との当たり判定
                        // FIXME:DX.VECTOR StartPos = DX.ConvScreenPosToWorldPos(DX.VGet(_XBuf, _YBuf, 0.0f));
                        // FIXME:DX.VECTOR EndPos = DX.ConvScreenPosToWorldPos(DX.VGet(_XBuf, _YBuf, 1.0f));
                        // FIXME:DX.MV1_COLL_RESULT_POLY HitPoly = DX.MV1CollCheck_Line(mh.modelHandle, -1, StartPos, EndPos);
                        // FIXME:if (HitPoly.HitFlag == 1){
                            if (XBuf - _XBuf < 0)
                            {
                                mh.modelX += 0.5f;
                            }
                            if (XBuf - _XBuf > 0)
                            {
                                mh.modelX -= 0.5f;
                            }
                            if (YBuf - _YBuf < 0)
                            {
                                mh.modelY -= 0.5f;
                            }
                            if (YBuf - _YBuf > 0)
                            {
                                mh.modelY += 0.5f;
                            }
                            //当たり判定したモデルを移動する
                            DX.MV1SetPosition(mh.modelHandle, DX.VGet(mh.modelX, mh.modelY, mh.modelZ));
                        // FIXME:}
                    }
                    XBuf = _XBuf;
                    YBuf = _YBuf;
                }
            }
            // マウスの右ボタンがクリックされているか
            if ((DX.GetMouseInput() & DX.MOUSE_INPUT_RIGHT) != 0)
            {
                int _XBuf;
                int _YBuf;
                if (0 == DX.GetMousePoint(out _XBuf, out _YBuf))
                {
                    if (XBuf - _XBuf < 0)
                    {
                        this.rotate(cameraX, cameraZ, out cameraX, out cameraZ, +ROTATE_SPEED, targetX, targetZ);//回転
                    }
                    if (XBuf - _XBuf > 0)
                    {
                        this.rotate(cameraX, cameraZ, out cameraX, out cameraZ, -ROTATE_SPEED, targetX, targetZ);//回転
                    }
                    if (YBuf - _YBuf < 0)
                    {
                        this.rotate(cameraY, cameraZ, out cameraY, out cameraZ, -ROTATE_SPEED, targetY, targetZ);//回転
                    }
                    if (YBuf - _YBuf > 0)
                    {
                        this.rotate(cameraY, cameraZ, out cameraY, out cameraZ, +ROTATE_SPEED, targetY, targetZ);//回転
                    }
                    System.Diagnostics.Trace.WriteLine(cameraX + "," + cameraY + "," + cameraZ);
                    DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(cameraX, cameraY, cameraZ), DX.VGet(targetX, targetY, targetZ)); // 第1引数の位置から第2引数の位置を見る角度にカメラを設置
                    XBuf = _XBuf;
                    YBuf = _YBuf;
                }
            }
        }

        // (x,y)の点を(mx,my)を中心にang角回転する
        private void rotate(float _x, float _y, out float x, out float y, float ang, float mx, float my)
        {
            float ox = _x - mx, oy = _y - my;
            _x =  ox * (float)Math.Cos(ang) + oy * (float)Math.Sin(ang);
            _y = -ox * (float)Math.Sin(ang) + oy* (float)Math.Cos(ang);
            x = _x + mx;
            y = _y + my;
        }

        public void setScale()
        {
            foreach (ModelHandle mh in this.modelHandles)
            {
                DX.MV1SetScale(mh.modelHandle, DX.VGet(scale, scale, scale));
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DX.DxLib_End(); // DxLibの終了処理
            icon.Dispose();

            // clean temporary directory
            if (this.temp_root_directory.Length != 0)
            {
                // delete directory
                System.IO.Directory.Delete(temp_root_directory, true);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None; // フォームの枠を非表示にする
            TransparencyKey = Color.FromArgb(1, 1, 1); // 透過色を設定
        }

        private void loadFiles()
        {
            List<Data.Model> models = modelData.getEnabledModels(this.directory);
            List<Data.Motion> motions = motionData.getMotions(this.directory);

            // cleanup
            foreach(ModelHandle mh in modelHandles)
            {
                // モデルハンドルの削除
                DX.MV1DeleteModel(mh.modelHandle);
            }
            if (this.temp_root_directory.Length != 0)
            {
                // delete directory
                System.IO.Directory.Delete(temp_root_directory, true);
            }
            // temporary directory
            this.temp_root_directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), this.generateRandomString(16));
            // create temporary directory
            System.IO.Directory.CreateDirectory(this.temp_root_directory);

            foreach (Data.Model model in models)
            {
                // filename
                string filename = model.FileName;
                // modeldir
                string model_directory = System.IO.Path.GetDirectoryName(filename);
                // temporary directory
                string model_temporary_directory = System.IO.Path.Combine(this.temp_root_directory, this.generateRandomString(16));
                // copy
                this.DirectoryCopy(model_directory, model_temporary_directory);

                // copy motion file
                string guid = model.MotionGuid;
                foreach (Data.Motion motion in motions)
                {
                    if (guid == motion.Guid)
                    {
                        string target = System.IO.Path.Combine(model_temporary_directory, System.IO.Path.GetFileNameWithoutExtension(model.FileName) + "000" + System.IO.Path.GetExtension(motion.FileName));
                        System.IO.File.Copy(motion.FileName, target, true);
                        break;
                    }
                }

                // 3Dモデルの読み込み(モデル名+000から始まる連番のモーションファイルも同時に読み込まれる)
                // モーションを別々に読み込む方法がないので、ファイル指定で読もうと思ったら、一回MV1DeleteModelでモデルを削除してモデルを読み直す
                // モデルを読む時にモーションファイルをリネームして同じディレクトリに構築する

                ModelHandle mh = new ModelHandle();
                mh.modelHandle = DX.MV1LoadModel(System.IO.Path.Combine(model_temporary_directory, System.IO.Path.GetFileName(filename)));
                if (mh.modelHandle == -1)
                {
                    MessageBox.Show(Properties.Resources.LoadModelFailed, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new Exception();
                }
                //TODO: モデルをランダムに移動しておく必要があるかもしれない
                mh.modelX = 0.0f;
                mh.modelY = 0.0f;
                mh.modelZ = 0.0f;
                DX.MV1SetPosition(mh.modelHandle, DX.VGet(mh.modelX, mh.modelY, mh.modelZ));

                // モーション設定
                // 今回は000～002まであるので、0,1,2がそれぞれ000,001,002のモーションに対応しています。今回は2番(3つ目のモーション)を選択
                //attachIndex = DX.MV1AttachAnim(modelHandle, 2, -1, DX.FALSE); // モーションの選択
                mh.attachIndex = DX.MV1AttachAnim(mh.modelHandle, 0, -1, DX.FALSE); // モーションの選択

                mh.totalTime = DX.MV1GetAttachAnimTotalTime(mh.modelHandle, mh.attachIndex); // モーションの総再生時間を取得
                mh.playTime = 0.0f; // モーションの再生位置

                // モデル全体のコリジョン情報を構築
                DX.MV1SetupCollInfo(mh.modelHandle, -1, 8, 8, 8);

                modelHandles.Add(mh);
                this.setScale();
            }
        }

        /// <summary>
        /// ランダムな文字列を生成する
        /// </summary>
        /// <param name="length">生成する文字列の長さ</param>
        /// <returns>生成された文字列</returns>
        private string generateRandomString(int length)
        {
            string passwordChars = "0123456789abcdefghijklmnopqrstuvwxyz";

            StringBuilder sb = new StringBuilder(length);
            Random r = new Random();

            for (int i = 0; i < length; i++)
            {
                //文字の位置をランダムに選択
                int pos = r.Next(passwordChars.Length);
                //選択された位置の文字を取得
                char c = passwordChars[pos];
                //パスワードに追加
                sb.Append(c);
            }

            return sb.ToString();
        }

        public void DirectoryCopy(string sourcePath, string destinationPath)
        {
            System.IO.DirectoryInfo sourceDirectory = new System.IO.DirectoryInfo(sourcePath);
            System.IO.DirectoryInfo destinationDirectory = new System.IO.DirectoryInfo(destinationPath);

            //コピー先のディレクトリがなければ作成する
            if (destinationDirectory.Exists == false)
            {
                destinationDirectory.Create();
                destinationDirectory.Attributes = sourceDirectory.Attributes;
            }

            //ファイルのコピー
            foreach (System.IO.FileInfo fileInfo in sourceDirectory.GetFiles())
            {
                //同じファイルが存在していたら、常に上書きする
                fileInfo.CopyTo(destinationDirectory.FullName + @"\" + fileInfo.Name, true);
            }

            //ディレクトリのコピー（再帰を使用）
            foreach (System.IO.DirectoryInfo directoryInfo in sourceDirectory.GetDirectories())
            {
                DirectoryCopy(directoryInfo.FullName, destinationDirectory.FullName + @"\" + directoryInfo.Name);
            }
        }
    }

    public class ModelHandle
    {
        public int modelHandle { get; set; }
        public float totalTime { get; set; }
        public float playTime { get; set; }
        public float modelX = 0.0f;
        public float modelY = 0.0f;
        public float modelZ = 0.0f;
        public int attachIndex;
    }
}
