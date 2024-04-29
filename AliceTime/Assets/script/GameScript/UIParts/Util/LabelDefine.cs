using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// UIに付与するラベルを定義(csvから自動で辞書作りたい)
/// これが付与されてると勝手に言語設定で変換してくれる
/// </summary>
public static class LabelDefine{

    public enum TEXTLABEL
    {
        LOADING,
        START,
    }

    public struct LanguageSet
    {
        public string Japanese;
        public string English;
        public LanguageSet(string _Japan,string _Eng)
        {
            Japanese = _Japan;
            English = _Eng;
        }
    }

    private static Dictionary<TEXTLABEL, LanguageSet> languageDictionary = new Dictionary<TEXTLABEL, LanguageSet>()
    {
        {TEXTLABEL.LOADING,new LanguageSet("よみこみちゅう","Now Loading")},
        {TEXTLABEL.START,new LanguageSet("スタート","Start")},
    };

    public static string GetString(TEXTLABEL labelEnum)
    {
        if (OptionManager.Instance.GetLanguateType() == OptionManager.LanguageType.Japanese)
        {
            return languageDictionary[labelEnum].Japanese;
        }
        else
        {
            return languageDictionary[labelEnum].English;
        }
    }

}
