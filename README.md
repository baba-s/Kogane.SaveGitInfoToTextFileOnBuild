# Kogane Save Git Info To Text File On Build

ビルド時に Git の情報を Resources フォルダのテキストファイルに書き込むエディタ拡張

## 使用例

![2022-10-15_162223](https://user-images.githubusercontent.com/6134875/195974617-08b648be-d8b2-4ea3-9c56-d999fbee0226.png)

Project Settings で Git の情報を書き込むテキストファイルの保存場所や  
書き込むテキストファイルのフォーマットを設定します

```cs
using UnityEngine;

public class Example : MonoBehaviour
{
    private void Awake()
    {
        var textAsset = Resources.Load<TextAsset>( "git" );
        Debug.Log( textAsset != null ? textAsset.text : "" );
    }
}
```

そして上記のようなコードを記述することで  
ビルド時における Git のブランチ名やコミットログを取得できます
