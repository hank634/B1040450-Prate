using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LADY : MonoBehaviour {
    private Animator ani;//動畫元件
    private Rigidbody rig; //剛體元件
    [Header("速度"), Range(0f, 80f)]
    public float speed = 1.5f;
    [Header("旋轉速度"), Range(1f, 100f)]
    public float turn = 1.5f;
    [Header("血量"), Range(100, 500)]
    public float hp = 100;

    [Header("動畫控制器")]
    [Header("跑步")]
    public string parRun = "RUN";
    [Header("攻擊")]
    public string parAtk = "ATK";
    [Header("受傷")]
    public string parHurt = "HURT";
    [Header("跳躍")]
    public string parJump = "JUMP";
    [Header("死亡")]
    public string parStun = "STUN";

    //屬性 可以設定權限取得get,設定set
    //修飾詞 類型 名稱 {取得 設定}
    public int MyProperty { get; set; }
    //public int MyProp1 { get; }//唯讀

        public bool isatk
    {
        get
        {
            return ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊");
        }
    }

    public bool ishurt
    {
        get
        {
            return ani.GetCurrentAnimatorStateInfo(0).IsName("受傷");
        }
    }

    private void Start()
    {
        ani=GetComponent<Animator>();//動畫元件欄位=取得元件<泛型>();
        rig = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        //判斷為動畫狀態
        //print("是否為攻擊動畫:" + isatk);
        //print("是否為受傷動畫:" + ishurt);

        if (isatk || ishurt) return;//跳出

        Turn();
        atk();
    }


    private void FixedUpdate()//FixedUpdate 1格執行0.002秒(使用物理的寫這裡)
    {
        if (isatk || ishurt) return;//跳出

        run();
        jump();
    }

    //觸發事件:碰到勾選IsTrigger碰撞器開始時候執行一次
    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (other.tag=="trap")
        {
            hurt();
        }
    }

    //定義方法
    //修飾詞 傳回類型 方法名稱 (參數){敘述}
    //void無傳回

    /// <summary>
    /// 跑步
    /// </summary>
    private void run()
    {
        //動畫跑步-按下前後:Vertical,或者上下,Horizontal
        ani.SetBool(parRun,Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal")!=0);
        //rig.AddForce(0, 0, Input.GetAxisRaw("Vertical")*speed);//以世界座標移動
        //rig.AddForce(transform.forward*Input.GetAxisRaw("Vertical") * speed);//以區域座標移動
        //前方 transform.forward(0,0,1)
        //右方 transform.right(1,0,0)
        //上方 transform.up(0,1,0)
        rig.AddForce(transform.forward * Input.GetAxisRaw("Vertical") * speed + transform.right * Input.GetAxisRaw("Horizontal") * speed);
    }

    /// <summary>
    /// 左右旋轉
    /// </summary>
    
private void Turn()
    {
        float x = Input.GetAxis("Mouse X");// 滑鼠左右,左 -1,右 1
        print("玩家滑鼠X:" + x);
        //Time.deltaTime 一帧的時間
        transform.Rotate(0, x*turn*Time.deltaTime, 0);
    }


    /// <summary>
    /// 攻擊
    /// </summary>
    private void atk()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            ani.SetTrigger(parAtk);
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    private void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ani.SetTrigger(parJump);
    }

    /// <summary>
    ///受傷
    /// </summary>
    private void hurt()
    {
        ani.SetTrigger(parHurt);
        hp -= 20;
        if (hp <= 0) dead();
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void dead()
    {
        ani.SetBool(parStun, true);
        //此腳本
        //enabled啟動
        this.enabled = false;

    }






}
