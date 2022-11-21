using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mygame
{

    public interface ISceneController                      //加载场景
    {
        void LoadResources();
    }
    public interface IUserAction                          //用户互动会发生的事件
    {
        void gamestart();                                    //游戏开始
        // void gunshoot();                                    //发射子弹
        int Check();                                       //检测游戏结束
        int getround();
    }

    public class SSDirector : System.Object
    {
        private static SSDirector _instance;
        public ISceneController CurrentScenceController { get; set; }
        public static SSDirector GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SSDirector();
            }
            return _instance;
        }
    }

    public class ScoreController : System.Object{//分数控制器
        private static ScoreController scoreController;
        int score;
        public static ScoreController GetScoreController () {
            if (scoreController == null) {
                scoreController = new ScoreController ();
            }

            return scoreController;
        }
        
        public int getscore(){
            return score;
        }

        public void addscore(int point){
            score += point;
        }

        public void gamestart(){
            score  = 0;
        }

    }

    public class DiskFactory : System.Object {//飞碟工厂
        public DiskModel diskPrefab;
        private static DiskFactory diskFactory;
        List<DiskModel> usingDisks;
        List<DiskModel>[] uselessDisks;

        public static DiskFactory getFactory () {
            if (diskFactory == null) {
                diskFactory = new DiskFactory ();
                diskFactory.uselessDisks = new List<DiskModel>[4];
                diskFactory.usingDisks = new List<DiskModel>();
                for(int i = 0;i<4;i++){
                    diskFactory.uselessDisks[i] = new List<DiskModel>();
                }
            }

            return diskFactory;
        }

        public void prepareDisks (int diskCount,System.Random r) {
            for (int i = 0; i < diskCount; i++) {
                int kind = r.Next()%4;//飞碟种类随机
                if (uselessDisks[kind].Count == 0) {
                    DiskModel disk = new DiskModel(kind);
                    usingDisks.Add (disk);
                } else {
                    DiskModel disk = uselessDisks[kind][0];
                    uselessDisks[kind].RemoveAt (0);
                    usingDisks.Add (disk);
                }
            }

        }

        public void recycleall(){
            for(int i = usingDisks.Count-1;i>=0;i--)recycleDisk(i);
        }

        public void recycleDisk (int index) {
            uselessDisks[usingDisks[index].getkind()].Add (usingDisks[index]);
            usingDisks.RemoveAt (index);
        }

        public List<DiskModel> GetDiskModels(){
            return usingDisks;
        }
    }

    public class DiskModel{
        GameObject disk;
        static Vector3[] position = {new Vector3(-20,10,20),new Vector3(15,5,10),new Vector3(-20,0,0),new Vector3(20,20,15),new Vector3(0,0,10)};
        static Vector3[] force = {new Vector3(5,5,-4),new Vector3(-8,5,0),new Vector3(8,8,5),new Vector3(-5,0,-1.5f),new Vector3(0,8,0)};
        static Vector3[] rota = {new Vector3(5,5,-5),new Vector3(10,0,1),new Vector3(-3,5,4),new Vector3(1,-12,-8),new Vector3(-3,-6,4)};
        int grade;
        int diegrade;
        int kind;
        public int statu;

        Diskbehaviour diskbehaviour;
        
        public DiskModel(int k){
            kind = k;
            string kstr = "disk";
            kstr += kind.ToString();
            disk = Object.Instantiate(Resources.Load(kstr, typeof(GameObject))) as GameObject;
            if(kind== 0){
                grade = 5;
                diegrade = -2;
            }
            else if(kind == 1){
                grade = 10;
                diegrade = -2;
            }
            else if(kind == 2){
                grade = 20;
                diegrade = -2;
            }
            else if(kind == 3){
                grade = -10;
                diegrade = 5;
            }
            statu = 0;
            diskbehaviour = disk.AddComponent(typeof(Diskbehaviour)) as Diskbehaviour;
            diskbehaviour.setdisk(this);
        }

        public void setdisk(int sp,System.Random r){//改变飞碟速度
            int rp = r.Next()%position.GetLength(0);
            disk.transform.position = position[rp];
            Rigidbody rigidbody;
            rigidbody = disk.GetComponent<Rigidbody>();
            //启动刚体
            rigidbody.WakeUp();
            rigidbody.useGravity = true;
            //添加瞬间力
            float disksp = 1;
            for(int i = 1;i<sp;i++)disksp *= 1.1f;
            rigidbody.AddForce(force[rp]*Random.Range(5, 8)*disksp/5, ForceMode.Impulse);
            //添加旋转力
            rigidbody.AddTorque(rota[rp] * 10);
        }

        public int getgrade(){
            return grade;
        }
        
        public int getdiegrade(){
            return diegrade;
        }

        public int getkind(){
            return kind;
        }

        public Vector3 getpos(){
            return disk.transform.position;
        }

        public int status(){
            if(disk.transform.position.y<-3)statu = 1;
            return statu;
        }

        public void destroy(){
            statu = 0;
            disk.GetComponent<Rigidbody>().Sleep();
            disk.GetComponent<Rigidbody>().useGravity = false;
            disk.transform.position = new Vector3(0f, -99f, 0f);
        }
    }

    public class Diskbehaviour : MonoBehaviour{
        DiskModel disk;
        public void setdisk(DiskModel disk){
            this.disk = disk;
        }
        void OnTriggerEnter(Collider collider) {
            if(collider.tag == "Finish"){
                disk.statu = 2;
            }
        }
    }

    public class GunModel{
        GameObject gun;
        Gunbehaviour gunbehaviour;
        public GunModel(){
            gun = Object.Instantiate(Resources.Load("gun", typeof(GameObject)),new Vector3(1,0,-9),Quaternion.identity) as GameObject;
            gunbehaviour = gun.AddComponent(typeof(Gunbehaviour)) as Gunbehaviour;
            
        }
        public void gamestart(){
            gunbehaviour.setaction(1);
        }
        public void gameend(){
            gunbehaviour.setaction(0);
            gun.transform.rotation = Quaternion.identity;
        }
    }

    public class Gunbehaviour: MonoBehaviour{
        public Vector3 mousePos;
        int action = 0;
        float firesp  = 100;
        GameObject bullet;
        GameObject fire;
        void Start(){
            bullet = Object.Instantiate(Resources.Load("bullet", typeof(GameObject)),new Vector3(0f, -99f, -99f),Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>().useGravity = false;
            fire = transform.GetChild(1).gameObject;
        }

        void Update(){
            if(action==1){
                Vector3 offset = mousePos - Input.mousePosition;
                transform.Rotate(Vector3.up * offset.x *-0.1f, Space.World);//左右旋转
                transform.Rotate(Vector3.right * -offset.y *-0.1f, Space.World);//上下旋转

                mousePos = Input.mousePosition;
                if(Input.GetMouseButtonDown(0)){
                    GameObject newbullet = Instantiate(bullet,fire.transform.position,transform.rotation);
                    Destroy(newbullet, 3.0f);
                    Rigidbody clone = newbullet.GetComponent<Rigidbody>();
                    clone.velocity = transform.TransformDirection(Vector3.forward*firesp); 
                }
            }
        }

        public void setaction(int action){
            this.action = action;
            if(action == 1)mousePos = new Vector3(Screen.width / 2,Screen.height/2-10,0);
        }
    }
}


