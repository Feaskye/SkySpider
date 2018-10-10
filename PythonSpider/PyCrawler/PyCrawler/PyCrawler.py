import keyword
import json


class A(object):  
    def foo(self,x):  
    #类实例方法  
        #for key in keyword.kwlist:
        #    print("--"+key)

        #tuple =('s','d',555)
        print(3**5)
        student = {'Tom', 'Jim', 'Mary', 'Tom', 'Jack', 'Rose'}
        if 'Tomg' in student:
            print('Tomg 在集合中')
        elif 'Jim' in student:
            print("Jim in it")
        else:
            print('Tomg 不在集合中')

            '''
            hello world
            '''

            """
            hello 
            """

        #age = str(input("请输入你家狗狗的名字"))
        #print("你家狗狗的名字为:"+age)

        #a = set('abracadabra')
        #b = set('alacazam')

        #print(a - b)     # a和b的差集
        #print(a | b)     # a和b的并集
        #print(a & b)    # a和b的交集
        #print(a ^ b)     # a和b中不同时存在的元素
        #print ("executing foo")
  
        #dict={}
        #dict[1]="sdf"
        #dict['ee']="ll"
        #print(dict.keys())
        #print(dict.values())

        #flag=True
        #if flag:
        #    print("dddd------true")

        list=[1,2,3,4]
        it = iter(list) 
        print (next(it))

    @classmethod  
    def class_foo(cls,x):  
    #类方法  
        str="{\"name\":\"123\",\"des\":\"123\"}"
        data = json.loads(str)

        print ("executing class_foo")
  
    @staticmethod  
    def static_foo(x):  
    #静态方法  
        print ("executing static_foo")



a = A()  
# a.foo(1)     #print   : executing foo(<__main__.A object at 0xb77d67ec>,1)
   
a.class_foo(1)   #executing class_foo(<class '__main__.A'>,1)  
A.class_foo(1)    #executing class_foo(<class '__main__.A'>,1)  
   
a.static_foo(1)    #executing static_foo(1)  
A.static_foo(1)   #executing static_foo(1)















