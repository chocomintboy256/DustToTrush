using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dusts : MonoBehaviour
{
    const int BONUS_APPEAR_RATE = 230;
    [SerializeField] GameObject normalDust;
    [SerializeField] GameObject rainbowDust;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject box;
    public GameObject Box { get { return box; } private set {} }
    private List<Dust> dusts = new List<Dust>();


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            dusts.Add(child.GetComponent<Dust>());
            ResetPosition(child); 
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TrashInDustDestory()
    {
        List<Dust> trushInDusts = dusts.Where(x => x.IsTrushIn).ToList();
        trushInDusts.ForEach((Dust x) => {
            DustDestroy(x);
        });
    }

    void DustDestroy(Dust dust) {
        GameMain gameMain = mainCamera.GetComponent<GameMain>();
        gameMain.TrushIn(dust.Score);
        if (dusts.Count == 1) {
            GenerateDusts();
            GameMain.ins.AddTimeBonus();
        }
        dusts.Remove(dust);
        Destroy(dust.gameObject);
    }

    void GenerateDusts()
    {
        int count = Random.Range(3, 5);
        for (int i = 0; i < count; i++)
        {
            Dust dust = CreateDust();
            dusts.Add(dust);
        }
    }
    Dust CreateDust()
    {
        bool nextBonusFg = getNextDustBonus();
        GameObject nextGameObject = nextBonusFg ? rainbowDust : normalDust;
        GameObject newDust = Instantiate(nextGameObject, GetRandomVector3(), Quaternion.identity, transform);
        return newDust.GetComponent<Dust>();
    }
    bool getNextDustBonus()
    {
        return Random.Range(1, BONUS_APPEAR_RATE) == 1;
    }

    Vector3 GetRandomVector3()
    {
        Vector3 vec = new Vector3(
            Random.Range(-2.3f, 2.3f),
            Random.Range(-2.8f, 2.8f),
            0
        );
        return vec;
    }

    public void ResetPosition(Transform tr)
    {
        tr.position = GetRandomVector3();
    }


}
