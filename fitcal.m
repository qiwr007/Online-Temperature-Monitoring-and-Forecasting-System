function [ ObjFitness,outputfinal,WBNew] = fitcal(WB,net,hiddennum,inputn,outputn,flag)
%WB,input,个体
%net,input,神经网络
%hiddennum,input,隐含层节点数
%inputn,input,训练输入数据
%outputn,input,训练输出数据
%error,output,个体适应度值
[m,n]=size(WB);%计算个体的数目
[inputnum,SampleNum]=size(inputn);%计算输入的节点数
outputnum=size(outputn,1);%计算输出的节点数
ObjFitness=zeros(m,1);%初始化适应度值
outputfinal=zeros(m*outputnum,SampleNum);%初始化输出
WBNew=WB;
for j=1:m
    %对神经网络权值进行初始化
    w1=WB(j,1:inputnum*hiddennum);
    B1=WB(j,inputnum*hiddennum+1:inputnum*hiddennum+hiddennum);
    w2=WB(j,inputnum*hiddennum+hiddennum+1:inputnum*hiddennum+hiddennum+hiddennum*outputnum);
    B2=WB(j,inputnum*hiddennum+hiddennum+hiddennum*outputnum+1:...
        inputnum*hiddennum+hiddennum+hiddennum*outputnum+outputnum);
    if n~=inputnum*hiddennum+hiddennum+hiddennum*outputnum+outputnum
        error('神经网络节点数输入错误!')
    end
    %网络进化参数
    if flag==1
        net.trainParam.epochs=2000;
         net.trainParam.lr=0.1;
          net.trainParam.goal=0.00001;
           net.trainParam.show=100;
           %网络权值赋值
           net.IW{1,1}=reshape(w1,hiddennum,inputnum);
           net.LW{2,1}=reshape(w2,outputnum,hiddennum);
           net.b{1}=B1';
           net.b{2}=B2';
           %网络训练
           net=train(net,inputn,outputn);
           outputfinal((j-1)*outputnum+1:j*outputnum,:)=sim(net,inputn);
           %计算新的神经网络权值
           WBNew(j,:)=[reshape(net.IW{1,1},1,inputnum*hiddennum),reshape(net.b{1},1,hiddennum),...
               reshape(net.LW{2,1},1,hiddennum*outputnum),reshape(net.b{2},1,outputnum)];
    else
           %网络权值赋值
           net.IW{1,1}=reshape(w1,hiddennum,inputnum);
           net.LW{2,1}=reshape(w2,outputnum,hiddennum);
           net.b{1}=B1';
           net.b{2}=B2';
           %网络输出
          outputfinal((j-1)*outputnum+1:j*outputnum,:)=sim(net,inputn);
    end
    %计算目标函数值
    errorObj=outputfinal((j-1)*outputnum+1:j*outputnum,:)-outputn;
    ObjFitness(j)=sum(abs(errorObj(:)));
end
ObjFitness=ObjFitness';

end

