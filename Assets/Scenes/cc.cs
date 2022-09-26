using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cc : MonoBehaviour
{
    private GameObject[,] ChessTable = new GameObject[3,3];
    private GameObject[,] chess= new GameObject[3,3];
    private int[,] state = new int[3,3];
    private int counter;
    void Start(){
        string m1 = "grid1",m2 = "grid2";
        for(int i = 0;i < 3;i++){
            for(int j = 0;j <3;j++){
                ChessTable[i,j] = Instantiate(Resources.Load((i+j)%2==1?m1:m2), new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                ChessTable[i,j].transform.parent = this.transform;
            }
        }
        Restart();
    }
    //
    void Restart(){
        counter = 0;
        for(int i = 0;i < 3;i++)
            for(int j = 0;j <3;j++){
                state[i,j] = 0;
                GameObject.Destroy(chess[i,j]);
            }
                
    }

    void createchess(){
        string m1 = "chess1",m2 = "chess2";
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //判断是否检测到了
        if(Physics.Raycast(ray, out hit)){
            //画条线看的更清楚
            Debug.DrawLine(ray.origin, hit.point);
            //既然是射线，那我们获得的肯定是碰撞体了，所以没有碰撞体这个组件的是检测不到的！
            GameObject grid = hit.collider.gameObject;
            int x  = (int)grid.transform.position.x,y = (int)grid.transform.position.z;
            if(grid.transform.position.x>0.9&&grid.transform.position.x<1.1)x = 1;
            if(state[x,y]==0){
                chess[x,y] = Instantiate(Resources.Load(counter%2==1?m1:m2), new Vector3(x,1,y), Quaternion.identity) as GameObject;
                //同时我们还可以利用counter来判断回合
                chess[x,y].transform.parent = this.transform;
                state[x,y] = counter%2==0?1:-1;
                counter += 1;
            }
        }
    }

    int judge(){
        int win = 0,t;
        //主对角线
        t = state[0,0];
        for(int i = 1;i<=2;i++){
            win = t;
            if(t != state[i,i]){
                win = 0;break;
            }
        }
        if(win==1||win==-1)return win;
        //斜对角线
        t = state[0,2];
        for(int i = 1;i<=2;i++){
            win = t;
            if(t != state[i,2-i]){
                win = 0;break;
            }
        }
        if(win==1||win==-1)return win;
        //竖线
        for(int j = 0;j<=2;j++){
            t = state[0,j];
            for(int i = 1;i<=2;i++){
                win = t;
                if(t != state[i,j]){
                    win = 0;break;
                }
            }
            if(win==1||win==-1)return win;
        }
        //横线
        for(int j = 0;j<=2;j++){
            t = state[j,0];
            for(int i = 1;i<=2;i++){
                win = t;
                if(t != state[j,i]){
                    win = 0;break;
                }
            }
            if(win==1||win==-1)return win;
        }
        if(counter==9)return 2;
        return 0;
    }

    void OnGUI(){
        //生成一个菜单栏
        GUI.Box (new Rect (10, 10, 100, 90), "菜单");
        if (GUI.Button (new Rect (20, 40, 80, 20), "新游戏")) {
            this.Restart();
        }
        int r = judge();
        if(r==1||r==-1||r==2){//win
            string m;
            if(r==1)m="你赢了";
            else if(r==-1)m = "你输了";
            else m = "平局";
            GUI.Box (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 50, 200, 100), m);
            if (GUI.Button (new Rect (Screen.width / 2 - 80, Screen.height / 2, 160, 20), "重开"))
            this.Restart ();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))createchess();
    }
}
