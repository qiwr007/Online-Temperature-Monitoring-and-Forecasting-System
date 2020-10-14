function [BA_output,e,t] = BA_Bpnet_fuction( traininputn0,trainoutputn0)
%UNTITLED4 Summary of this function goes here
%   Detailed explanation goes here
%对训练样本进行主成分分析
tic;
[inputnum,N]=size(traininputn0);
PCAinputNN=traininputn0;
% [A,PCAinput,latent]=princomp(traininputn0');
% Csum=cumsum(latent);
% nPCA=find(Csum/Csum(inputnum)>0.85,1);%搜索总能量大于85%的部分
% PCAinputNN=PCAinput(:,1:nPCA)';
% disp(['经过主成分分析，输入样本维数由',num2str(inputnum),'维降到',num2str(nPCA),'维.']);
% inputnum=nPCA;
outputnum=size(trainoutputn0,1);%得到输出的维数
hiddennum=10;
TF1='logsig';
TF2='tansig';
% net=newff(PCAinputNN,trainoutputn0,hiddennum);
net=newff(traininputn0,trainoutputn0,hiddennum);
%net=newff(minmax(PCAinputNN),[hiddennum outputnum],{TF1 TF2},'trainlm');
%设置蝙蝠种群参数
e=0;
acc=0;
n=20;%初始种群个体数
N=100;%迭代次数
A=1;%最大脉冲音量
r=1;%最大脉冲率
para=[n N A r inputnum hiddennum outputnum];
%进行种群选择
[best,fmin,N_iter]=init_bat(para,net,PCAinputNN,trainoutputn0);
disp(['Number of evaluations: ',num2str(N_iter)]);
disp(['Best =',num2str(best)]); 
disp( ['fmin=',num2str(fmin)]);
%%利用神经网络进一步优化
[ObjVbest,outputfinal,net]=BA_fitcal(best,net,hiddennum,PCAinputNN,trainoutputn0,1);
[W,N]=size(traininputn0);
figure('name','神经网络输出与期望输出的比较','numbertitle','off');
SampleNum=1:N;
plot(SampleNum,round(abs(outputfinal(1,:))),'-or',SampleNum,trainoutputn0(1,:),'linewidth',2);
% plot(SampleNum,trainoutputn0,'linewidth',2)
% hold on
% plot(SampleNum,abs(outputfinal(1,:)),'-or','linewidth',3)
grid on;
set(gca,'fontsize',16);
h=legend('神经网络输出','期望输出');
set(h,'fontsize',16);
xlabel('迭代次数','fontsize',16);
ylabel('第一维输出值','fontsize',16);
title('蝙蝠算法神经网络输出与期望第一维输出的比较','fontsize',16);
grid on;
 t=toc;
 e=0;
for k=1:N
%    if(round(abs(outputfinal(k)))<=trainoutputn0(k))
%        e = e+1;
%  if((abs(outputfinal(1,k))-trainoutputn0(k))<=1.5)
%             e=e+1;
%         e = e+(abs(outputfinal(1,k))-trainoutputn0(k))^2;
 zhixin=abs(outputfinal(1,k))/trainoutputn0(k);
     if(zhixin >= 0.85 & zhixin <= 1.15)
         e=e+1;
   end
acc=e/N;
disp(['蝙蝠算法正确率为:',num2str(acc)]);
disp(['本程序运行时间为',num2str(t),'秒。']);
BA_output = abs(outputfinal(1,:));
end

