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

namespace DesktopCharacter
{
    // デスクトップマスコットの作り方
    // https://qiita.com/massoumen/items/2985a0fb30472b97a590

    public partial class Form1 : Form
    {
        /// <summary>
        /// モデルデータ
        /// </summary>
        private int modelHandle;

        private int attachIndex;
        private float totalTime;
        private float playTime;
        public float playSpeed { get; set; }

        float targetX = 0.0f, targetY = 10.0f, targetZ = 0.0f;
        float cameraX = 0.0f, cameraY = 10.0f, cameraZ = -30.0f;
        float modelX = 0.0f, modelY = 0.0f, modelZ = 0.0f;
        float ROTATE_SPEED = (float)Math.PI / 90;//回転スピード
        private int XBuf, YBuf;
        public float scale { get; set; }

        private NotifyIcon icon;
        public bool ignoreKeyAndMouse { get; set; }

        public Screen screen { get; set; }

        public Form1()
        {
            InitializeComponent();

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
            DX.DxLib_Init(); // DxLibの初期化処理
            DX.SetDrawScreen(DX.DX_SCREEN_BACK); // 描画先を裏画面に設定

            this.scale = 1.0f;
            // 3Dモデルの読み込み(モデル名+000から始まる連番のモーションファイルも同時に読み込まれる)
            // モーションを別々に読み込む方法がないので、ファイル指定で読もうと思ったら、一回MV1DeleteModelでモデルを削除してモデルを読み直す
            // モデルを読む時にモーションファイルをリネームして同じディレクトリに構築する
            //modelHandle = DX.MV1LoadModel(@"data\結月ゆかり_純_ver1.0\結月ゆかり_純.pmd");
            //modelHandle = DX.MV1LoadModel(@"data\Mirai_Akari_v1.0\MiraiAkari_v1.0.pmx");
            //modelHandle = DX.MV1LoadModel(@"data\Lat式ミクVer2.31\Lat式ミクVer2.31_White.pmd");
            modelHandle = DX.MV1LoadModel(@"data\kizunaai\kizunaai.pmx");
            if (modelHandle == -1)
            {
                MessageBox.Show("モデルのロードが正しく出来ませんでした", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception();
            }
            DX.MV1SetPosition(modelHandle, DX.VGet(modelX, modelY, modelZ));
            this.setScale();

            // モーション設定
            // 今回は000～002まであるので、0,1,2がそれぞれ000,001,002のモーションに対応しています。今回は2番(3つ目のモーション)を選択
            //attachIndex = DX.MV1AttachAnim(modelHandle, 2, -1, DX.FALSE); // モーションの選択
            attachIndex = DX.MV1AttachAnim(modelHandle, 0, -1, DX.FALSE); // モーションの選択

            totalTime = DX.MV1GetAttachAnimTotalTime(modelHandle, attachIndex); // モーションの総再生時間を取得
            playTime = 0.0f; // モーションの再生位置
            playSpeed = 0.2f; // モーションの再生位置を進める速度

            // カメラのセットアップ
            DX.SetCameraNearFar(0.1f, 1000.0f); // 奥行0.1-1000をカメラの描画範囲とする
            DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(cameraX, cameraY, cameraZ), DX.VGet(targetX, targetY, targetZ)); // 第1引数の位置から第2引数の位置を見る角度にカメラを設置

            XBuf = 0;
            YBuf = 0;

            TaskTray();
            this.Location = screen.Bounds.Location;
        }

        private void TaskTray()
        {
            // タスクトレイにアイコンを表示する
            icon = new NotifyIcon();
            icon.Icon = new Icon("if_girl-avatar_2903195.ico");
            icon.Visible = true;
            icon.Text = "DesktopCharacter";
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem menuItem0 = new ToolStripMenuItem();
            menuItem0.Text = "&設定";
            menuItem0.Click += new EventHandler(Option_Click);
            menu.Items.Add(menuItem0);

            ToolStripMenuItem menuItem1 = new ToolStripMenuItem();
            menuItem1.Text = "&終了";
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

            playTime += playSpeed; // 時間を進める
            // モーションの再生位置が終端まで来たら最初に戻す
            if (playTime >= totalTime)
            {
                playTime = 0.0f;
            }
            // モーションの再生位置を設定
            DX.MV1SetAttachAnimTime(modelHandle, attachIndex, playTime);

            DX.MV1DrawModel(modelHandle); // 3Dモデルの描画

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
                    if (XBuf - _XBuf < 0)
                    {
                        modelX += 0.5f;
                    }
                    if (XBuf - _XBuf > 0)
                    {
                        modelX -= 0.5f;
                    }
                    if (YBuf - _YBuf < 0)
                    {
                        modelY -= 0.5f;
                    }
                    if (YBuf - _YBuf > 0)
                    {
                        modelY += 0.5f;
                    }
                    DX.MV1SetPosition(modelHandle, DX.VGet(modelX, modelY, modelZ));
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
            DX.MV1SetScale(modelHandle, DX.VGet(scale, scale, scale));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DX.DxLib_End(); // DxLibの終了処理
            icon.Dispose();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None; // フォームの枠を非表示にする
            TransparencyKey = Color.FromArgb(1, 1, 1); // 透過色を設定
        }
    }
}
