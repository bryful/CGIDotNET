# CGIDotNET

C#で普通のCGIを作るためのライブラリ  
あと、JavaScriptでのWebアプリのライブラリ  
  
昔かなりCGIを作ってたけど、Delphiだったので基本的なところはライブラリ任せだったので、
C#で作ろうとすると基本的なところがわからなくて大変です。  
今回は基本的なライブラリを作って今後楽に作ろうと考えています。
  
実際作ってみると、意外と簡単ですね。  
  
JavaScriptでのWebアプリは２０年前はいまいち使えなかったのですが、今は十分使えますね。  
それも考慮に入れて作ってます。  

## NFsCGI_Test

CGIのテストをするためのプロジェクト

* NFsCgi.cs POST/GETの処理を行うクラス
* NFsHtml.cs htmlの処理を行うクラス

## FindFolder.CGI

CGIを使って自宅のPCのフォルダを検索するCGI。  
とりあえず簡単に作ってみたCGI。  

FindFolderDBの機能を内蔵させました。

### FindFolderDB
まともにフォルダ検索してたらレスポンスが遅かったので作ったフォルダのDBを作るもの。  
単純にフォルダのリストをjsonで書き出すWindowsアプリ。
最終的にはFindFolder.CGIに統合する予定  
FindFolder.CGIに機能を統合しました。  


## 動作

04WebServerを立ち上げてテストしています。
ビルドの設定でserverのフォルダへindex.cgiとリネームコピーさせてます。




## Dependency
Visual studio 2019 C#

## License
This software is released under the MIT License, see LICENSE

## Authors

bry-ful(Hiroshi Furuhashi)  
twitter:[bryful](https://twitter.com/bryful)  
Mail: bryful@gmail.com  

