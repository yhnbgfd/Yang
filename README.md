Yang
====

杨老师软件a+b

可能存在的问题：32位系统下程序最多调用2g内存，可能出现内存超出2g导致数据错乱。

2013.06.08

优化保存功能，每次保存前先gc，解决了多次保存大量占用内存的问题。

新问题：单次保存占用内存有所上升，800w条数据的时候已经突然2g。
