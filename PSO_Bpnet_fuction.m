function [ PSO_output,e,t] = PSO_Bpnet_fuction( traininputn0,trainoutputn0 )
%UNTITLED3 Summary of this function goes here
%   Detailed explanation goes here
%对训练样本进行主成分分析
[inputnum,N]=size(traininputn0);
PCAinputNN=traininputn0;
% [A,PCAinput,latent]=princomp(traininputn0');
% Csum=cumsum(latent);
% nPCA=find(Csum/Csum(inputnum)>0.85,1);%搜索总能量大于85%的部分
% PCAinputNN=PCAinput(:,1:nPCA)';
% save('PCAtrainData','PCAinputNN','trainoutputn0');%存储数据
% disp(['经过主成分分析，输入样本维数由',num2str(inputnum),'维降到',num2str(nPCA),'维.']);
% inputnum=nPCA;
outputn=trainoutputn0;
%设置微粒群参数
outputnum=size(trainoutputn0,1);%得到输出的维数
hiddennum=10;
NumPartical=20;%种群粒子数
c1=1.5;
c2=1.5;
e=0;
vmax=1;%速度上限
minerr=1e-7;%目标误差
wmax=0.95;
wmin=0.25;
MAXGEN=20;%最大迭代次数
NVAR=(inputnum+1)*hiddennum+(hiddennum+1)*outputnum;%所有权值和阈值
V=normrnd(0,1,NVAR,NumPartical);
X=normrnd(0,1,NVAR,NumPartical);%初始化粒子群的速度和位置
P=X;%设置初始位置为最优位置
TF1='logsig';
TF2='tansig';
% net=newff(PCAinputNN,outputn,hiddennum);
net=newff(traininputn0,outputn,hiddennum);
%net=newff(minmax(PCAinputNN),[hiddennum outputnum],{TF1 TF2},'trainlm');
%计算各个粒子的适应度
[ObjV,outputNN]=fitcal(P',net,hiddennum,PCAinputNN,trainoutputn0,0);
%求最优位置
[ObjVbest,minnum]=min(ObjV);
pbest=P(:,minnum);
%进入粒子群循环
traceObj=zeros(MAXGEN+2,2);%记录目标函数值变化的情况
traceObj(1,:)=ObjVbest*ones(1,2);%记录最优解与平均解对应的适应值
for gen=1:MAXGEN
    r=unifrnd(0,1,2,1);%生成两个[0,1]的随机数
    omega=0.9-gen/MAXGEN*0.5;%计算动态惯性权重
    V=omega*V+c1*r(1)*(P-X)+c2*r(2)*(pbest*ones(1,NumPartical)-X);%更新速度
    V=V.*(abs(V)<=vmax)+vmax*((V>vmax)-(V<-vmax));
    X=X+V;%更新位置
    %计算各粒子对应的适应度
    [ObjV1,outputNN]=fitcal(X',net,hiddennum,PCAinputNN,trainoutputn0,0);
    %求最优位置对应的最优适应度
    [ObjV,nump]=min([ObjV1,ObjV]);
%更新各个粒子的最优位置
numChooseX=find(nump==1);
P(:,numChooseX)=X(:,numChooseX);
end
%求总的最优位置
[ObjVbest,minnum]=min(ObjV);
pbest=P(:,minnum);
traceObj(gen+1,1)=ObjVbest;%记录目标函数值变化的情况
traceObj(gen+1,2)=sum(ObjV)/NumPartical;%记录最优解与平均解对应的适应值
disp(['目前运行到第',num2str(gen),'代.']);
%%利用神经网络进一步优化
[ObjVbest,outputfinal,net]=fitcal(pbest',net,hiddennum,PCAinputNN,trainoutputn0,1);
traceObj(MAXGEN+2,:)=ObjVbest*ones(1,2);
disp(['PSO神经网络最小误差方差为',num2str(ObjVbest)]);
figure('name','神经网络输出与期望输出的比较','numbertitle','off');
SampleNum=1:N;
plot(SampleNum,abs(outputfinal(1,:)),'-<m',SampleNum,trainoutputn0(1,:),'linewidth',2);
% plot(SampleNum,trainoutputn0,'linewidth',2)
% hold on
% plot(SampleNum,abs(outputfinal(1,:)),'-<m','linewidth',2)
grid on;
set(gca,'fontsize',16);
h=legend('神经网络输出','期望输出');
set(h,'fontsize',16);
xlabel('迭代次数','fontsize',16);
ylabel('第一维输出值','fontsize',16);
title('PSO神经网络输出与期望输出的比较','fontsize',16);
grid on;
t=toc;

for k=1:N
%    if(round(abs(outputfinal(k)))==trainoutputn0(k))
%        e = e+1;
%  if((abs(outputfinal(1,k))-trainoutputn0(k))<=0.5)
%             e=e+1;
%  e = e+(abs(outputfinal(1,k))-trainoutputn0(k))^2;
 zhixin=abs(outputfinal(1,k))/outputn(k);
     if(zhixin >= 0.85 & zhixin <= 1.15)
         e=e+1;
end
 acc=e/N;
disp(['粒子群算法正确率:',num2str(acc)]);
disp(['本程序运行时间为',num2str(t),'秒。'])
PSO_output=abs(outputfinal(1,:));
end

