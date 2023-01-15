using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardView : MonoBehaviour
{
    //功能
    GameObject Target = null;
    public bool isInView = false;//是否在扇形视野范围内
    public bool isRayCast = false;//是否通过射线能看到
    public bool isAlert = false;//是否警报
    float PerceiveRadius = 8f;//感知范围
    // float AlertTime = 10f;//警报时间
    // float WaitTime = 4f;//失去目标等待时间
    float LightRange = 0f;//灯光距离（在Start()中根据SpotLight参数设定）
    float LightIntensity = 0f;//灯光亮度（在Start()中根据SpotLight参数设定）
    float ViewAngle = 60f;//视野角度
 
    float TargetAngle = 0;
    float AngleDiff = 0;
 
    public GameObject m_Light;
 
    ///
    //角度差计算函数
    ///
 
    void CalculateAngle()
    {
        if (Target != null)
        {
            float AtanAngle = (Mathf.Atan((Target.transform.position.z - this.transform.position.z) /
            (Target.transform.position.x - this.transform.position.x))
            * 180.0f / 3.14159f);
            //Debug.Log (this.transform.rotation.eulerAngles+"   "+AtanAngle);
 
            //1象限角度转换
            if ((Target.transform.position.z - this.transform.position.z) > 0
               &&
            (Target.transform.position.x - this.transform.position.x) > 0
               )
            {
                TargetAngle = 90f - AtanAngle;
                //Debug.Log ("象限1 "+TargetAngle);
            }
 
            //2象限角度转换
            if ((Target.transform.position.z - this.transform.position.z) <= 0
               &&
            (Target.transform.position.x - this.transform.position.x) > 0
               )
            {
                TargetAngle = 90f + -AtanAngle;
                //Debug.Log ("象限2 "+TargetAngle);
            }
 
            //3象限角度转换
            if ((Target.transform.position.z - this.transform.position.z) <= 0
               &&
            (Target.transform.position.x - this.transform.position.x) <= 0
               )
            {
                TargetAngle = 90f - AtanAngle + 180f;
                //Debug.Log ("象限3 "+TargetAngle);
            }
 
            //4象限角度转换
            if ((Target.transform.position.z - this.transform.position.z) > 0
               &&
            (Target.transform.position.x - this.transform.position.x) <= 0
               )
            {
                TargetAngle = 270f + -AtanAngle;
                //Debug.Log ("象限4 "+TargetAngle);
            }
 
 
            //调整TargetAngle
            float OriginTargetAngle = TargetAngle;
            if (Mathf.Abs(TargetAngle + 360 - this.transform.rotation.eulerAngles.y)
               <
            Mathf.Abs(TargetAngle - this.transform.rotation.eulerAngles.y)
               )
            {
                TargetAngle += 360f;
            }
            if (Mathf.Abs(TargetAngle - 360 - this.transform.rotation.eulerAngles.y)
               <
            Mathf.Abs(TargetAngle - this.transform.rotation.eulerAngles.y)
               )
            {
                TargetAngle -= 360f;
            }
 
            //输出角度差
            AngleDiff = Mathf.Abs(TargetAngle - this.transform.rotation.eulerAngles.y);
            // Debug.Log("角度差:" + TargetAngle + "(" + OriginTargetAngle + ")-" + this.transform.rotation.eulerAngles.y + "=" + AngleDiff);
        }
    }
 
    //
    //感知视野的相关计算 判断isRayCast和isInView
    //
 
    void JudgeView()
    {
 
        //感知角度相关计算
        if (Target != null)
        {
            Debug.Log(Target);
            //指向玩家的向量计算
            Vector3 vec = new Vector3(Target.transform.position.x - this.transform.position.x,
                                    0f,
                                    Target.transform.position.z - this.transform.position.z);
            Debug.Log(this.transform.position);
            Debug.Log(Target.transform.position);
            Debug.Log(vec);
            
            //射线碰撞判断
            RaycastHit hitInfo;
            if (Physics.Raycast(this.transform.position, vec, out
                               hitInfo,20,LayerMask.GetMask("Player")|LayerMask.GetMask("Ground")))
            {
                GameObject gameObj = hitInfo.collider.gameObject;
                //Debug.Log("Object name is " + gameObj.name);
                if (gameObj.tag == "Player")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                {
                    Debug.Log("Seen!");
                    isRayCast = true;
                }
                else
                {
                    isRayCast = false;
                }
            }
 
            //画出碰撞线
            Debug.DrawLine(this.transform.position, hitInfo.point, Color.red, 1);
            //视野中的射线碰撞判断结束
 
            //视野范围判断
            //物体在范围角度内,警戒模式下范围为原来1.5倍
            if (AngleDiff * 2 <
               (isAlert ? ViewAngle * 1.5f : ViewAngle)
               )
            {
                isInView = true;
            }
            else
            {
                isInView = false;
            }
            // Debug.Log ("角度差 "+AngleDiff);
 
        }
 
    }
 
 
 
    // Use this for initialization
 

    void Start()

    {
        LightRange = m_Light.GetComponent<Light>().range;
        LightIntensity = m_Light.GetComponent<Light>().intensity;
        this.GetComponent<SphereCollider>().radius = PerceiveRadius;
 
    }
 
    // Update is called once per frame
 
    void Update()
    {
 
        //Debug.Log("state:" + state + " time_alert:" + time_alert);
 
        if (isAlert)
        {
 
            //警戒模式
            m_Light.GetComponent<Light>().range = LightRange * 2f;
            m_Light.GetComponent<Light>().color = new Color(0.784f, 0.317f, 0.203f,1f);
            m_Light.GetComponent<Light>().intensity = LightIntensity * 2f;
            m_Light.GetComponent<Light>().spotAngle = ViewAngle * 1.5f;
            this.GetComponent<SphereCollider>().radius = PerceiveRadius * 1.5f;
 
        }
        else
        {
 
            //正常模式
            m_Light.GetComponent<Light>().range = LightRange;
            m_Light.GetComponent<Light>().color = new Color(0.257f, 0.745f, 0.108f,1f);
            m_Light.GetComponent<Light>().intensity = LightIntensity;
            m_Light.GetComponent<Light>().spotAngle = ViewAngle;
            this.GetComponent<SphereCollider>().radius = PerceiveRadius;
 
        }

        //计算角度差
 
        CalculateAngle();
 
        //感知视野判断（判断isRayCast与isInView)
 
        JudgeView();
    }
    
    public GameObject getTarget(){
        return Target;
    }

    //玩家进入感知层
 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Target = other.gameObject;
            //提前计算角度差
            CalculateAngle();
        }
    }
 
    //玩家进入视野
 
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Target == null)
            {
                Target = other.gameObject;
            }
        }
    }
 
    //玩家离开感知层
 
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Target = null;
            isInView = false;
            isRayCast = false;
        }
    }
}
