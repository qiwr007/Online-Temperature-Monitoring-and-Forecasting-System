function [ ObjFitness,outputfinal,WBNew] = fitcal(WB,net,hiddennum,inputn,outputn,flag)
%WB,input,����
%net,input,������
%hiddennum,input,������ڵ���
%inputn,input,ѵ����������
%outputn,input,ѵ���������
%error,output,������Ӧ��ֵ
[m,n]=size(WB);%����������Ŀ
[inputnum,SampleNum]=size(inputn);%��������Ľڵ���
outputnum=size(outputn,1);%��������Ľڵ���
ObjFitness=zeros(m,1);%��ʼ����Ӧ��ֵ
outputfinal=zeros(m*outputnum,SampleNum);%��ʼ�����
WBNew=WB;
for j=1:m
    %��������Ȩֵ���г�ʼ��
    w1=WB(j,1:inputnum*hiddennum);
    B1=WB(j,inputnum*hiddennum+1:inputnum*hiddennum+hiddennum);
    w2=WB(j,inputnum*hiddennum+hiddennum+1:inputnum*hiddennum+hiddennum+hiddennum*outputnum);
    B2=WB(j,inputnum*hiddennum+hiddennum+hiddennum*outputnum+1:...
        inputnum*hiddennum+hiddennum+hiddennum*outputnum+outputnum);
    if n~=inputnum*hiddennum+hiddennum+hiddennum*outputnum+outputnum
        error('������ڵ����������!')
    end
    %�����������
    if flag==1
        net.trainParam.epochs=2000;
         net.trainParam.lr=0.1;
          net.trainParam.goal=0.00001;
           net.trainParam.show=100;
           %����Ȩֵ��ֵ
           net.IW{1,1}=reshape(w1,hiddennum,inputnum);
           net.LW{2,1}=reshape(w2,outputnum,hiddennum);
           net.b{1}=B1';
           net.b{2}=B2';
           %����ѵ��
           net=train(net,inputn,outputn);
           outputfinal((j-1)*outputnum+1:j*outputnum,:)=sim(net,inputn);
           %�����µ�������Ȩֵ
           WBNew(j,:)=[reshape(net.IW{1,1},1,inputnum*hiddennum),reshape(net.b{1},1,hiddennum),...
               reshape(net.LW{2,1},1,hiddennum*outputnum),reshape(net.b{2},1,outputnum)];
    else
           %����Ȩֵ��ֵ
           net.IW{1,1}=reshape(w1,hiddennum,inputnum);
           net.LW{2,1}=reshape(w2,outputnum,hiddennum);
           net.b{1}=B1';
           net.b{2}=B2';
           %�������
          outputfinal((j-1)*outputnum+1:j*outputnum,:)=sim(net,inputn);
    end
    %����Ŀ�꺯��ֵ
    errorObj=outputfinal((j-1)*outputnum+1:j*outputnum,:)-outputn;
    ObjFitness(j)=sum(abs(errorObj(:)));
end
ObjFitness=ObjFitness';

end

