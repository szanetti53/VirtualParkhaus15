using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstallationInfo : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text titleYearText;
    [SerializeField]
    private TMP_Text statementText;
    [SerializeField]
    private string artistName;
    [SerializeField]
    private string titleYear;
    [SerializeField]
    private string statement;

    public GameObject art;
    [SerializeField]
    private Transform position;

    void Start()
    {
        PopulateText();
        CreateArt();
        
    }
    public void PopulateText()
    {

        nameText.text = artistName;
        titleYearText.text = titleYear;
        statementText.text = statement;


    }

    public void CreateArt()
    {
        if(art!=null)
        {

            GameObject Placcard = Instantiate(art, position);
            Placcard.transform.localPosition = Vector3.zero;
        }
       
    }
}
