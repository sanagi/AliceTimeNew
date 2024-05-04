using System;
using System.Collections.Generic;

namespace InputSupport
{
    public class InputInfoManager<T> where T : InputParamBase<T>
    {

        private List<T> m_InputInfo;  // list of InputInfo
            
        public List<T> InputInfo
        {
            get { return m_InputInfo == null ? (m_InputInfo = new List<T>()) : m_InputInfo; }
        }
        public int InfoCount
        {
            get { return m_InputInfo == null ? 0 : m_InputInfo.Count; }
        }

        
        public void UpdateParam(T inputParam, Action endCallback) {
            // avoid null error.
            if (m_InputInfo == null) {
                m_InputInfo = new List<T>();
            }
            
            // update input parameter.
            updateInputParameter(inputParam);

            if (endCallback != null) {
                endCallback();
            }
        }

        public void UpdateParams(T[] inputParams, Action endCallback) {
            // avoid null error.
            if (m_InputInfo == null) {
                m_InputInfo = new List<T>();
            }

            // update input parameter.
            var index = 0;
            var length = inputParams.Length;
            while(index < length) {
                updateInputParameter(inputParams[index]);
                index++;
            }
        }

        public void Clear() {
            if (m_InputInfo == null) {
                return;
            }

            var index = 0;
            var length = m_InputInfo.Count;
            while (index < length) {
                if (m_InputInfo.Count <= index) {
                    break;
                } 
                m_InputInfo[index].Phase = InputPhase.Ended;
                updateInputParameter(m_InputInfo[index]);
                index++;
            }
        }


        private void updateInputParameter(T inputParam) {
            var sameInputInfo = getSameInput(inputParam);
            if (sameInputInfo != null && sameInputInfo.Phase == InputPhase.Missing) {
                m_InputInfo.Remove(sameInputInfo);
                return;
            }
            if (sameInputInfo == null) {
                m_InputInfo.Add(inputParam);
                return;
            }

            if (sameInputInfo.Phase != InputPhase.Ended) {
                if (sameInputInfo.Phase == InputPhase.Began) {
                    inputParam.Phase = InputPhase.Stay;
                }
                sameInputInfo.UpdateParam(sameInputInfo, inputParam);
            }
        }

        private T getSameInput(T inputInfo) {
            return m_InputInfo.Find(ei => ei.Id == inputInfo.Id);
        }
    }
}
