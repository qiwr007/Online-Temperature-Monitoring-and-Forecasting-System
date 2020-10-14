clear all;
close all;
clc;
tic;
input1=[0.5970 0.6769 0.1905 0.6561 0.5069 0.5194 0.2153 1.0000 0.7520 0.7804 0.9524 0.7765 0.5772 1.0000 0.9562 0.7234]';
input2=[0.7306 0.7545 1.0000 0.7522 0.6762 1.0000 1.0000 0.6865 0.2289 0.2849 0.6667 0.2652 0.0000 0.3972 0.6971 0.5943]';
input3=[0.0002 0.0382 0.4048 0.0216 0.5507 0.0949 0.4343 0.2684 0.2415 0.2701 0.9048 0.2598 0.9300 0.4965 0.9197 0.3975]';
input4=[0.3728 0.4373 0.0714 0.4094 0.6012 0.2775 0.0876 0.6742 0.8089 0.8567 0.5357 0.8383 0.5990 0.8659 0.5657 0.7930]';
input5=[1.0000 1.0000 0.3452 1.0000 0.0274 0.9387 0.3723 0.6045 0.7657 0.7870 0.5119 0.7883 0.1483 0.8126 0.5365 0.7311]';
input6=[0.3347 0.3624 0.0595 0.3538 0.7281 0.2428 0.0803 0.4631 0.4490 0.5469 0.0595 0.5106 0.2541 0.3341 0.0730 0.9743]';
input7=[0.1278 0.1840 0.0000 0.1647 1.0000 0.0445 0.0000 0.5451 0.2705 0.1798 0.3571 0.1759 0.3405 0.2248 0.3905 0.2234]';
input8=[0.0000 0.0000 0.1429 0.0000 0.5349 0.0000 0.1679 0.0000 0.1653 0.1832 0.3690 0.1767 0.3453 0.2248 0.3978 0.2664]';
input9=[0.4490 0.7184 0.2954 0.2834 0.4087 0.5743 0.1299 0.1157 0.3724 0.4017 0.4301 0.7201 0.3011 0.8957 0.6948 0.7695]';
input10=[0.3392 0.3761 0.5713 0.7092 0.2834 0.8117 1.0000 0.5150 0.2602 0.7117 0.2396 0.6702 0.4190 0.7499 0.7347 0.6578]';
input11=[0.5759 0.1947 0.5680 0.3496 0.3474 0.6621 0.7267 0.5768 0.1082 0.6222 1.0000 0.7521 0.3952 1.0000 0.7656 0.6334]';
input12=[0.4374 0.7421 0.1536 0.3589 0.2247 0.3231 0.3388 0.4171 0.4629 0.5748 0.4216 0.3071 0.2380 0.5555 0.4885 0.5610]';
input13=[0.1373 0.5137 0.9189 0.8517 1.0000 0.9681 0.8801 0.7493 0.0000 0.0000 0.5754 0.9346 0.4117 0.3857 0.4537 0.7410]';
input14=[0.6129 0.4484 0.1682 0.4036 0.0911 0.0313 0.2113 0.0000 0.3062 0.4789 0.7405 0.6503 0.2338 0.6876 0.7260 0.5496]';
input15=[0.9375 0.1429 0.0000 0.0000 0.0717 0.0825 0.1819 0.0360 0.0372 0.7722 0.6920 1.0000 0.2524 0.5309 0.8430 0.9574]';
input16=[1.0000 1.0000 0.1378 0.0043 0.0000 0.0000 0.0000 0.0220 0.4056 0.3157 0.4749 0.7200 0.2794 0.2724 0.8746 1.0000]';
inputn0=[input1 input2 input3 input4 input5 input6 input7 input8 input9 input10 input11 input12 input13 input14 input15 input16];%输入样本
outputn0=[1;1;2;2;3;3;4;4;1;1;2;2;3;3;4;4]';
% testNum=10:2:16;
% testinputn0=inputn0(:,testNum);
% testoutputn0=outputn0(:,testNum);%q确定测试样本
% trainNum=setdiff(1:16,testNum);
% traininputn0=inputn0(:,trainNum);
% trainoutputn0=outputn0(:,trainNum);%确定训练样本
traininputn0=inputn0;
trainoutputn0=outputn0;
% outputn=trainoutputn0;

%遗传算法
[GA_output,e1,t1] = GA_Bpnet_fuction(traininputn0,trainoutputn0);
%BA算法
[BA_output,e2,t2] = BA_Bpnet_fuction( traininputn0,trainoutputn0);
%PSO算法
% PSO_output = 0;
% t3=0;
[PSO_output,e3,t3] = PSO_Bpnet_fuction(traininputn0,trainoutputn0);
figure('name','神经网络输出与期望输出的比较','numbertitle','off');
SampleNum=1:16;
figure(1)
plot(SampleNum,trainoutputn0,SampleNum,GA_output,SampleNum,PSO_output,SampleNum,BA_output,'linewidth',2);
plot(SampleNum,trainoutputn0,'linewidth',2)
hold on
plot(SampleNum,GA_output,'-dk','linewidth',2)
hold on
plot(SampleNum,PSO_output,'-<m','linewidth',2)
hold on
plot(SampleNum,BA_output,'-or','linewidth',2)
set(gca,'fontsize',12);
h=legend('期望输出','GA神经网络输出','PSO神经网络输出','BA神经网络输出');
grid on
set(h,'fontsize',12);
xlabel('迭代次数','fontsize',16);
ylabel('输出值','fontsize',16);
title('三种算法神经网络输出与期望输出的比较','fontsize',16);
figure('name','神经网络输出与期望输出拟合曲线比较','numbertitle','off');
% n=5;
% len=length(SampleNum);
% A1=polyfit(SampleNum,trainoutputn0,n);
% y1=polyval(A1,SampleNum);
% plot(SampleNum,y1,'linewidth',2)
% hold on
% A2=polyfit(SampleNum,GA_output,n);
% y2=polyval(A2,SampleNum);
% plot(SampleNum,y2,'-dk','linewidth',2)
% RMES(1)=sqrt(sum((y2-y1).^2)/(len-1));
% hold on
% A3=polyfit(SampleNum,PSO_output,n);
% y3=polyval(A3,SampleNum);
% plot(SampleNum,y3,'-<m','linewidth',2)
% RMES(2)=sqrt(sum((y3-y1).^2)/(len-1));
% % hold on
% % A4=polyfit(SampleNum,BA_output,n);
% % y4=polyval(A4,SampleNum);
% % RMES(3)=sqrt(sum((y4-y1).^2)/(len-1));
% % plot(SampleNum,y4,'-or','linewidth',2)
% set(gca,'fontsize',12);
% h=legend('期望输出','GA神经网络输出','PSO神经网络输出','BA神经网络输出');
% grid on
% set(h,'fontsize',12);
% xlabel('迭代次数','fontsize',16);
% ylabel('输出值','fontsize',16);
% title('三种算法神经网络输出与期望输出的比较','fontsize',16);
t=[t1 t2 t3]
e=[e1 e2 e3]