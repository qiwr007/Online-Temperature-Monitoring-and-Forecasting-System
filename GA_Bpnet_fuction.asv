function [ GA_output,e,t] = GA_Bpnet_fuction( traininputn0,trainoutputn0)
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here
%对训练样本进行主成分分析
tic;
outputn=trainoutputn0;
[inputnum,N]=size(traininputn0);
% [A,PCAinput,latent]=princomp(traininputn0');
% Csum=cumsum(latent);
% nPCA=find(Csum/Csum(inputnum)>0.85,1);%搜索总能量大于85%的部分
% inputn=PCAinput(:,1:nPCA)';
inputn=traininputn0;
% disp(['经过主成分分析，输入样本维数由',num2str(inputnum),'维降到',num2str(nPCA),'维.']);
% inputnum=nPCA;
outputnum=size(trainoutputn0,1);%得到输出的维数
e=0;
%初始化种群与网络参数
LB=-3;UB=3;
hiddennum=10;
NVAR=inputnum*hiddennum+hiddennum+hiddennum*outputnum+outputnum;%变量的维数
NIND=20;
MAXGEN=99;
PRICE=20;
GGAP=0.9;
trace=zeros(MAXGEN+1,2);
%建立区域描述器
FieldD=[rep(PRICE,[1,NVAR]);rep([LB;UB],[1,NVAR]);rep([1;0;1;1],[1,NVAR])];
Chrom=crtbp(NIND,NVAR*PRICE);%创建初始种群，将种群转换为二进制序列
gen=0;
WB=bs2rv(Chrom,FieldD);
%初始化神经网络
TF1='logsig';
TF2='tansig';
% net=newff(inputn,outputn,hiddennum);
net=newff(traininputn0,outputn,hiddennum);
%net=newff(minmax(inputn),[hiddennum outputnum],{TF1 TF2},'trainlm');
%计算适应度
 [ObjV,outputNN]=GABP(WB,net,hiddennum,inputn,outputn,0)%计算初始种群个体的目标函数值

while gen<MAXGEN
    FitnV=ranking(ObjV);%分配适应度
    SelCh=select('sus',Chrom,FitnV,GGAP);%选择
    SelCh=recombin('xovsp',SelCh,0.7);%重组
    SelCh=mut(SelCh);%变异
    WB=bs2rv(SelCh,FieldD);%将种群转为十进制序列
    gen=gen+1;
    trace(gen,1)=min(ObjV);
    trace(gen,2)=sum(ObjV)/length(ObjV);
end
%输出最优解及其对应的及其对应的自变量的十进制值
[minObjV,minnum]=min(ObjV);
WB=bs2rv(Chrom,FieldD);
WBOpt=WB(minnum,:);
%利用梯度下降法对神经网路权值与阈值在进行一次优化，并输出优化结果
%[ObjOpt,outputfinal,WBOpt]=GABP(WBOpt,net,hiddennum,inputn,outputn,1)%计算最优化值的目标函数值
[ObjOpt,outputfinal,WBOpt]=GABP(WBOpt,net,hiddennum,inputn,outputn,1)
trace(MAXGEN+1,1)=ObjOpt;
trace(MAXGEN+1,2)=ObjOpt;
figure('name','神经网络输出与期望输出的比较','numbertitle','off');
SampleNum=1:N;
plot(SampleNum,abs(outputfinal(1,:)),'-dk',SampleNum,outputn(1,:),'linewidth',2);
% plot(SampleNum,trainoutputn0,'linewidth',2)
% hold on
% plot(SampleNum,abs(outputfinal(1,:)),'-.k','linewidth',3)
grid on;
set(gca,'fontsize',16);
h=legend('神经网络输出','期望输出');
set(h,'fontsize',16);
xlabel('迭代次数','fontsize',16);
ylabel('输出值','fontsize',16);
title('遗传算法神经网络输出与期望输出的比较','fontsize',16);
t=toc;
e=0;
for k=1:N
%    if(round(abs(outputfinal(k)))<=outputn(k))
%        e = e+1;
%      if((abs(outputfinal(1,k))-outputn(k))<=0.5)
%             e=e+1;
% // e = e+(abs(outputfinal(1,k))-outputn(k))^2;
     zhixin=abs(outputfinal(1,k))/outputn(k);
     if(zhixin >= 0.85 & zhixin <= 1.15)
         e=e+1;
end
 acc=e/N;
disp(['遗传算法算法正确率为:',num2str(acc)]);
disp(['本程序的运行时间为',num2str(t),'秒。']);
GA_output=abs(outputfinal(1,:));
end

