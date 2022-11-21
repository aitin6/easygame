using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame;
public class Controllor : MonoBehaviour, ISceneController, IUserAction
{
    public static System.Random rand;
    UserGUI user_gui;
    DiskFactory diskFactory;

    ScoreController scoreController;
    List<DiskModel> diskModels;

    public int round;
    GunModel gun;

    void Start ()
    {
        //设置导演
        SSDirector director = SSDirector.GetInstance();
        //设置场景
        director.CurrentScenceController = this;
        //新建View
        user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        //设置分数控制器
        scoreController = ScoreController.GetScoreController();
        //设置飞碟工厂
        diskFactory =DiskFactory.getFactory();
        //设置飞碟模型
        diskModels = diskFactory.GetDiskModels();
        //设置随机种子
        rand = new System.Random((int) System.DateTime.Now.Ticks & 0x0000FFFF);
        LoadResources();
    }
	
    public void LoadResources()
    {
        gun = new GunModel();
        
    }
// 单击运行开始游戏：

// 每次从第一回合开始，玩家按“开始游戏”发射飞碟；
// 玩家通过鼠标点击打飞碟
// 随着回合数增加，飞碟数量增加，飞碟飞行速度变快；
// 当玩家分数低于0分或回合数大于十时，游戏结束。
    public void gamestart(){
        round = 0;
        scoreController.gamestart();
        gun.gamestart();
        user_gui.sign = 1;
    }

    public void gamestop(){
        diskFactory.recycleall();
        gun.gameend();
    }
    void Update(){
        if(user_gui.sign==1){
            checkdisk();
            //检查是否分数为负数
            user_gui.sign = Check();
            if(user_gui.sign==1&&diskModels.Count==0){
                round++;
                if(round<=10)sentdisk();
            }
            //检查是否全局
            user_gui.sign = Check();
            if(user_gui.sign!=1)gamestop();
        }
    }

    public void checkdisk(){
        for(int i = diskModels.Count-1;i>=0;i--){
            if(diskModels[i].status()==0)continue;
            string effect;
            if(diskModels[i].status()==1){
                scoreController.addscore(diskModels[i].getdiegrade());
                effect = "disappear";
            }
            else{
                scoreController.addscore(diskModels[i].getgrade());
                if(diskModels[i].getkind()==3)effect = "death";
                else effect = "ok";
            }
            Destroy(Object.Instantiate(Resources.Load(effect, typeof(GameObject)),diskModels[i].getpos(),Quaternion.identity) as GameObject,3);
            diskModels[i].destroy();
            DiskFactory.getFactory().recycleDisk(i);
        }
    }

    public void sentdisk(){
        int num = round/3+1;//飞碟数量
        int speed = round/2+1;//飞碟速度
        diskFactory.prepareDisks(num,rand);
        for(int i = 0;i< diskModels.Count;i++){
            diskModels[i].setdisk(speed,rand);
        }

    }

    public int Check()
    {
        if(scoreController.getscore()<0)return 2;
        if(round>10)return 3;
        return 1;
    }

    public int getround(){
        return round;
    }
}

