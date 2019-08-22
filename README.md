# Road Fighter项目简介
该项目简单复制了经典游戏Road Fighter。游戏玩法和经典版本非常类似，但是因为改成了网络对战模式，游戏的胜负机制被修改了。

# 网络架构解决方案
该游戏使用了Photon Cloud网络游戏解决方案。玩家配对是通过Photon Cloud自动匹配实现的。每个房间是一个第一个玩家为“服务器玩家”，第二个进入的玩家会变成“副”玩家。游戏的很多调控由“服务器玩家“进行判定和调度。

# Deployment平台
该游戏为WebGL/HTML5开发。WebGL Template由一位热心的开发者提供，该template可以去除Unity Logo并且让窗口自由缩放。

# 未来开发
游戏项目的主要脚本都可以在roadfighter/RoadFighter2D/Assets/Scripts/找到。
