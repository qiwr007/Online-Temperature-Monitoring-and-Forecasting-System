function [ GA_output,e,t] = GA_Bpnet_fuction( traininputn0,trainoutputn0)
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here
%��ѵ�������������ɷַ���
tic;
outputn=trainoutputn0;
[inputnum,N]=size(traininputn0);
% [A,PCAinput,latent]=princomp(traininputn0');
% Csum=cumsum(latent);
% nPCA=find(Csum/Csum(inputnum)>0.85,1);%��������������85%�Ĳ���
% inputn=PCAinput(:,1:nPCA)';
inputn=traininputn0;
% disp(['�������ɷַ�������������ά����',num2str(inputnum),'ά����',num2str(nPCA),'ά.']);
% inputnum=nPCA;
outputnum=size(trainoutputn0,1);%�õ������ά��
e=0;
%��ʼ����Ⱥ���������
LB=-3;UB=3;
hiddennum=10;
NVAR=inputnum*hiddennum+hiddennum+hiddennum*outputnum+outputnum;%������ά��
NIND=20;
MAXGEN=99;
PRICE=20;
GGAP=0.9;
trace=zeros(MAXGEN+1,2);
%��������������
FieldD=[rep(PRICE,[1,NVAR]);rep([LB;UB],[1,NVAR]);rep([1;0;1;1],[1,NVAR])];
Chrom=crtbp(NIND,NVAR*PRICE);%������ʼ��Ⱥ������Ⱥת��Ϊ����������
gen=0;
WB=bs2rv(Chrom,FieldD);
%��ʼ��������
TF1='logsig';
TF2='tansig';
% net=newff(inputn,outputn,hiddennum);
net=newff(traininputn0,outputn,hiddennum);
%net=newff(minmax(inputn),[hiddennum outputnum],{TF1 TF2},'trainlm');
%������Ӧ��
 [ObjV,outputNN]=GABP(WB,net,hiddennum,inputn,outputn,0)%�����ʼ��Ⱥ�����Ŀ�꺯��ֵ

while gen<MAXGEN
    FitnV=ranking(ObjV);%������Ӧ��
    SelCh=select('sus',Chrom,FitnV,GGAP);%ѡ��
    SelCh=recombin('xovsp',SelCh,0.7);%����
    SelCh=mut(SelCh);%����
    WB=bs2rv(SelCh,FieldD);%����ȺתΪʮ��������
    gen=gen+1;
    trace(gen,1)=min(ObjV);
    trace(gen,2)=sum(ObjV)/length(ObjV);
end
%������Ž⼰���Ӧ�ļ����Ӧ���Ա�����ʮ����ֵ
[minObjV,minnum]=min(ObjV);
WB=bs2rv(Chrom,FieldD);
WBOpt=WB(minnum,:);
%�����ݶ��½���������·Ȩֵ����ֵ�ڽ���һ���Ż���������Ż����
%[ObjOpt,outputfinal,WBOpt]=GABP(WBOpt,net,hiddennum,inputn,outputn,1)%�������Ż�ֵ��Ŀ�꺯��ֵ
[ObjOpt,outputfinal,WBOpt]=GABP(WBOpt,net,hiddennum,inputn,outputn,1)
trace(MAXGEN+1,1)=ObjOpt;
trace(MAXGEN+1,2)=ObjOpt;
figure('name','�������������������ıȽ�','numbertitle','off');
SampleNum=1:N;
plot(SampleNum,abs(outputfinal(1,:)),'-dk',SampleNum,outputn(1,:),'linewidth',2);
% plot(SampleNum,trainoutputn0,'linewidth',2)
% hold on
% plot(SampleNum,abs(outputfinal(1,:)),'-.k','linewidth',3)
grid on;
set(gca,'fontsize',16);
h=legend('���������','�������');
set(h,'fontsize',16);
xlabel('��������','fontsize',16);
ylabel('���ֵ','fontsize',16);
title('�Ŵ��㷨�������������������ıȽ�','fontsize',16);
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
disp(['�Ŵ��㷨�㷨��ȷ��Ϊ:',num2str(acc)]);
disp(['�����������ʱ��Ϊ',num2str(t),'�롣']);
GA_output=abs(outputfinal(1,:));
end

