using UnityEngine;

namespace Alice
{
    /// <summary>
    /// マテリアルのタイリングをTransformのスケールに同期してくれるスクリプト
    /// </summary>
    public class ClockworkMaterialAutoScale : MonoBehaviour
    {
        /// <summary>どのスケールを使うかの設定</summary>
        public enum ConvertType
        {
            None,
            LocalX,
            LocalY,
            LocalZ,
        }

        /// <summary>操作対象のレンダラー</summary>
        [SerializeField] private Renderer _targetRenderer = null;

        /// <summary>変換するとき どれとどれをつなぐか U軸</summary>
        [SerializeField] private ConvertType _convertTypeU = ConvertType.LocalX;
        /// <summary>変換するとき どれとどれをつなぐか V軸</summary>
        [SerializeField] private ConvertType _convertTypeV = ConvertType.LocalZ;

        /// <summary>変換するときのスケール</summary>
        [SerializeField] private Vector2 _scaleToTiling = new Vector2(1, 1);

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            //Rendererを取得
            if (null == _targetRenderer)
            {
                _targetRenderer = GetComponentInChildren<Renderer>();
            }

            //スケールを算出
            var localScale = transform.localScale;
            Vector2 convScale = _targetRenderer.material.mainTextureScale;
            if (ConvertType.None != _convertTypeU)
            {
                convScale.x = _convertTypeU switch
                {
                    ConvertType.LocalX => localScale.x,
                    ConvertType.LocalZ => localScale.z,
                    _ => localScale.y,
                };
            }
            if (ConvertType.None != _convertTypeV)
            {
                convScale.y = _convertTypeV switch
                {
                    ConvertType.LocalX => localScale.x,
                    ConvertType.LocalZ => localScale.z,
                    _ => localScale.y,
                };
            }
            //マテリアルに反映
            _targetRenderer.material.mainTextureScale = new Vector2(convScale.x * _scaleToTiling.x, convScale.y * _scaleToTiling.y);

            //終わったら消える
            Destroy(this);
        }
    }
}
