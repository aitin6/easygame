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
        void MoveBoat();                                   //移动船
        void Restart();                                    //重新开始
        void MoveRole(RoleModel role);                     //移动角色
        int Check();                                       //检测游戏结束
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

    public class LandModel
    {
        GameObject land;                                //陆地对象
        Vector3[] positions;                            //保存每个角色放在陆地上的位置
        int land_sign;                                  //到达陆地标志为-1，开始陆地标志为1
        RoleModel[] roles = new RoleModel[6];           //陆地上有的角色

        public LandModel(string land_mark)
        {
            positions = new Vector3[] {new Vector3(8.5f,2,2.5f), new Vector3(6.5f,2,1.5f), new Vector3(9.5f,2,0),
                new Vector3(5.5f,2,-2.5f), new Vector3(7.5f,2,-0.5f), new Vector3(8,2,-3f)};
            if (land_mark == "start")
            {
                land = Object.Instantiate(Resources.Load("Land", typeof(GameObject)), new Vector3(8, 1, 0), Quaternion.identity) as GameObject;
                land_sign = 1;
            }
            else
            {
                land = Object.Instantiate(Resources.Load("Land", typeof(GameObject)), new Vector3(-8, 1, 0), Quaternion.identity) as GameObject;
                land_sign = -1;
            }
        }

        public int GetEmptyNumber()                      //得到陆地上哪一个位置是空的
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null)
                    return i;
            }
            return -1;
        }

        public int GetLandSign() { return land_sign; }

        public Vector3 GetEmptyPosition()               //得到陆地上空位置
        {
            Vector3 pos = positions[GetEmptyNumber()];
            pos.x = land_sign * pos.x;                  //因为两个陆地是x坐标对称
            return pos;
        }

        public void AddRole(RoleModel role)             
        {
            roles[GetEmptyNumber()] = role;
        }

        public RoleModel DeleteRoleByName(string role_name)      //离开陆地
        { 
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null && roles[i].GetName() == role_name)
                {
                    RoleModel role = roles[i];
                    roles[i] = null;
                    return role;
                }
            }
            return null;
        }

        public int[] GetRoleNum()
        {
            int[] count = { 0, 0 };                    //count[0]是牧师数，count[1]是魔鬼数
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null)
                {
                    if (roles[i].GetSign() == 0)
                        count[0]++;
                    else
                        count[1]++;
                }
            }
            return count;
        }

        public void Reset()
        {
            roles = new RoleModel[6];
        }
    }

    public class BoatModel  
    {
        GameObject boat;                                          
        Vector3[] start_empty_pos;                                    //船在开始陆地的空位位置
        Vector3[] end_empty_pos;                                      //船在结束陆地的空位位置                           
        Click click;
        Move move;

        Transform maincamera;
        int boat_sign = 1;                                                     //船在开始还是结束陆地

        RoleModel[] roles = new RoleModel[2];                                  //船上的角色

        public BoatModel(Transform camera)
        {
            boat = Object.Instantiate(Resources.Load("Boat", typeof(GameObject)), new Vector3(2, 1, 0), Quaternion.identity) as GameObject;
            boat.name = "boat";
            this.maincamera = camera;
            move = boat.AddComponent(typeof(Move)) as Move;
            click = boat.AddComponent(typeof(Click)) as Click;
            click.SetBoat(this);
            start_empty_pos = new Vector3[] { new Vector3(2.2f, 2, -1), new Vector3(2.2f, 2, 1) };
            end_empty_pos = new Vector3[] { new Vector3(-2.2f, 2, -1), new Vector3(-2.2f, 2, 1) };
        }
        
        public bool IsEmpty()
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null)
                    return false;
            }
            return true;
        }

        public void BoatMove()
        {
            if (boat_sign == -1)
            {
                move.boatmove(new Vector3(2, 1, 0),maincamera);
                boat_sign = 1;
            }
            else
            {
                move.boatmove(new Vector3(-2, 1, 0),maincamera);
                boat_sign = -1;
            }
        }


        

        public int GetBoatSign(){ return boat_sign;}
        
        public int GetMove(){ return move.getmovesign();}

        public RoleModel DeleteRoleByName(string role_name)
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null && roles[i].GetName() == role_name)
                {
                    RoleModel role = roles[i];
                    roles[i] = null;
                    return role;
                }
            }
            return null;
        }

        public int GetEmptyNumber()
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public Vector3 GetEmptyPosition()
        {
            Vector3 pos;
            if (boat_sign == -1)
                pos = end_empty_pos[GetEmptyNumber()];
            else
                pos = start_empty_pos[GetEmptyNumber()];
            return pos;
        }


        public void AddRole(RoleModel role)
        {
            roles[GetEmptyNumber()] = role;
        }

        public GameObject GetBoat(){ return boat; }
        

        public void Reset()
        {
            if (boat_sign == -1)
                BoatMove();
            roles = new RoleModel[2];
            maincamera.rotation = Quaternion.Euler(0,0,0);
        }

        public int[] GetRoleNumber()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null)
                    continue;
                if (roles[i].GetSign() == 0)
                    count[0]++;
                else
                    count[1]++;
            }
            return count;
        }
    }

    public class RoleModel 
    {
        GameObject role;
        int role_sign;             //0为牧师，1为恶魔
        Click click;
        Move move;  
        bool on_boat;              //是否在船上       
        LandModel land_model = (SSDirector.GetInstance().CurrentScenceController as Controllor).start_land;

        public RoleModel(string role_name)
        {
            if (role_name == "priest")
            {
                role = Object.Instantiate(Resources.Load("Priest", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, -90, 0)) as GameObject;
                role_sign = 0;
            }
            else
            {
                role = Object.Instantiate(Resources.Load("Devil", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, -90, 0)) as GameObject;
                role_sign = 1;
            }
            move = role.AddComponent(typeof(Move)) as Move;
            click = role.AddComponent(typeof(Click)) as Click;
            click.SetRole(this);
        }

        public int GetSign() { return role_sign;}

        public int GetMove(){return move.getmovesign();}

        public LandModel GetLandModel(){return land_model;}
        public string GetName() { return role.name; }
        public bool IsOnBoat() { return on_boat; }
        public void SetName(string name) { role.name = name; }
        public void SetPosition(Vector3 pos) { role.transform.position = pos; }

        //从初始陆地上看 (0,-90,0)向前 0向右 90向后 180向左
        public void GoLand(LandModel land)
        {  
            //解除人和船的固定
            role.transform.parent = null;
            land_model = land;
            on_boat = false;
        }

        public void movetoland(LandModel land){
            move.movetoland(land);
        }
        public void GoBoat(BoatModel boat)
        {
            //将人固定在船上
            role.transform.parent = boat.GetBoat().transform;
            land_model = null;          
            on_boat = true;
        }

        public void movetoboat(BoatModel boat){
            move.movetoboat(boat);
        }
        public void Reset()
        {
            land_model = (SSDirector.GetInstance().CurrentScenceController as Controllor).start_land;
            GoLand(land_model);
            
            role.transform.rotation = Quaternion.Euler(0,-90,0);
            SetPosition(land_model.GetEmptyPosition());
            land_model.AddRole(this);
        }
    }

    public class Move : MonoBehaviour//移动辅助类
    {
               
        int move_sign = 0;                        //0是不动，1z轴移动，2x轴移动
        float rotaspeed = 200.0f; //转向速度
        float mspeed = 5;//移动速度

        Transform maincamera;
        int kind = 0;//移动方式

        Vector3 mid_position;//中间的位置
        Vector3 end_position;//最终的位置

        Quaternion mid_qua;//移动向中间的朝向

        Quaternion end_qua;//移动向最终的朝向

        Quaternion final_qua;//站稳后的朝向

        Quaternion cameraq;//摄像头的旋转角度

        void Update()
        {
            if(kind == 1)roleUpdate();
            else if(kind == 2) boatUpdate();
        }
        void boatUpdate(){
            if(move_sign==1){
                //渐慢的镜头旋转
                maincamera.rotation = Quaternion.RotateTowards(maincamera.rotation, cameraq, rotaspeed*Time.deltaTime);
                if(rotaspeed>0)rotaspeed -= 120*Time.deltaTime>rotaspeed?rotaspeed: 120*Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, end_position,mspeed * Time.deltaTime);
                if(transform.position==end_position)move_sign=0;
            }
        }
        
        public void boatmove(Vector3 position,Transform camera){
            end_position = position;
            maincamera = camera;
            //摄像头翻转
            if(camera.rotation==Quaternion.Euler(0,0,0))cameraq = Quaternion.Euler(180,0,180);
            else cameraq = Quaternion.Euler(0,0,0);
            kind = 2;
            move_sign=1;
            rotaspeed = 200;
            mspeed = 2;
        }
        void roleUpdate(){
            if(move_sign == 1){// 移动前转向
                transform.rotation = Quaternion.RotateTowards(transform.rotation, mid_qua, rotaspeed*Time.deltaTime);
                if(transform.rotation==mid_qua)move_sign=2;
            }
            else if(move_sign == 2){//移动到中间
                transform.position = Vector3.MoveTowards(transform.position, mid_position, mspeed * Time.deltaTime);
                if (transform.position == mid_position)move_sign=3;
            }
            else if(move_sign == 3){//在中间转向
                transform.rotation = Quaternion.RotateTowards(transform.rotation, end_qua, rotaspeed*Time.deltaTime);
                if(transform.rotation==end_qua)move_sign=4;
            }
            else if(move_sign == 4){//移动到最终目的地
                transform.position = Vector3.MoveTowards(transform.position, end_position, mspeed * Time.deltaTime);
                if (transform.position == end_position)move_sign=5;
            }
            else if(move_sign == 5){//在最终目的地转向
                transform.rotation = Quaternion.RotateTowards(transform.rotation, final_qua, rotaspeed*Time.deltaTime);
                if(transform.rotation==final_qua)move_sign=0;
            }
        }
        // -90前 0右 90后 180左 
        public void movetoland(LandModel land){
            Vector3 position = land.GetEmptyPosition();
            mid_position = new Vector3(position.x,transform.position.y,transform.position.z);
            end_position = new Vector3(position.x,transform.position.y,position.z);
            if(land.GetLandSign() == 1){//走下初始大陆上的转向方式
                mid_qua = Quaternion.Euler(0,90,0);
                if(position.z>transform.position.z)end_qua= Quaternion.Euler(0,0,0);
                else end_qua = Quaternion.Euler(0,180,0);
                final_qua  = Quaternion.Euler(0,-90,0);
            }
            else{//走下对面大陆上的转向方式
                mid_qua = Quaternion.Euler(0,-90,0);
                if(position.z>transform.position.z)end_qua= Quaternion.Euler(0,0,0);
                else end_qua = Quaternion.Euler(0,180,0);
                final_qua  = Quaternion.Euler(0,90,0);
            }
            kind = 1;
            move_sign = 1;
        }

        public void movetoboat(BoatModel boat){
            Vector3 position = boat.GetEmptyPosition();
            mid_position = new Vector3(transform.position.x,transform.position.y,position.z);
            end_position = new Vector3(position.x,transform.position.y,position.z);
            if(boat.GetBoatSign() == 1){//船在初始大陆上的转向方式
                if(position.z>transform.position.z)mid_qua = Quaternion.Euler(0,0,0);
                else mid_qua = Quaternion.Euler(0,180,0);
                end_qua = final_qua = Quaternion.Euler(0,-90,0);
            }
            else{//船在对面大陆上的转向方式
                if(position.z>transform.position.z)mid_qua = Quaternion.Euler(0,0,0);
                else mid_qua = Quaternion.Euler(0,180,0);
                end_qua = final_qua = Quaternion.Euler(0,90,0);
            }
            kind = 1;
            move_sign = 1;
        }

        public int getmovesign(){
            return move_sign;
        }
    }

    public class Click : MonoBehaviour//点击辅助类
    {
        IUserAction action;
        RoleModel role = null;
        BoatModel boat = null;
        public void SetRole(RoleModel role)
        {
            this.role = role;
        }
        public void SetBoat(BoatModel boat)
        {
            this.boat = boat;
        }
        void Start()
        {
            action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
        }
        void OnMouseDown()
        {
            if (boat == null && role == null) return;
            if (boat != null&&boat.GetMove()==0)
                action.MoveBoat();
            else if(role != null&&role.GetMove()==0)
                action.MoveRole(role);
        }
    }
}


