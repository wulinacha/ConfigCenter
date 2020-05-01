projectname="configcenter";
gitpath="https://github.com/wulinacha/ConfigCenter.git";
workpath="/home/share/configcenter";
dllpath="/home/share/configcenter/lib";
version="v1.0";

//����Ӧ�ó����������
applicationname="${params.applicationname}";//Ӧ�ó�����
gitbranch="${params.gitbranch}";//git��֧����
evn="${params.evn}";//Ӧ�ó������л��� ���� Development/Staging/Production
evnlowercase="${params.evn}".toLowerCase();//Ӧ�ó������л���,Сд
myapplcationpoint="${params.applcationpoint}";//Ӧ�ó���˿� 
mybuildpath="${workpath}/${params.buildpath}";//ִ�б���Ŀ¼ ���� /share/wms/src/Api/WMS.WebApi
myapplicationtype="${applicationtype}".toLowerCase();//Ӧ�ó������� ���� Console/Web
buildnode=GetBuildNode(evn);//��ȡ���������ǩ

/////// ���빹������Ҫ��������������ɾ��񣬽��������͵�˽�в֣�
node (buildnode)
{
    stage('��ȡ����'){
        dir(workpath){
           git branch: gitbranch,
           url: gitpath
       }
       dir(workpath){
            sh '''git submodule init
            git submodule update'''
       }
    }
    stage('����'){
        dir(mybuildpath){
            sh '''rm bin/publish -rf
            dotnet publish -c Release -f netcoreapp3.1 -o bin/publish
            '''
        }
    }
    stage('����') {
        dir(mybuildpath){
            sh 'docker build -t ${applicationname} .'
        }
    }
}
///////// end

///////// ������Ҫ�����Ǵ�˽�вֻ��߱��ػ�ȡ����ͨ��������������
if(evn=='Development'){
    node ('for_ubuntu_16'){
          DeployApplication();
    }
}
if(evn=='Staging'){
  node ('for_aliyun_001')
  {
       DeployApplication();
	   //ExecuteTest();
  }
}
if(evn=='Production'){
  node ("for_aliyun_001"){
    DeployApplication();
  }
}
//////// end

//////// ����
//��ȡ���������ǩ
def GetBuildNode(environmental){
	if(environmental=='Development'){
		buildnode="for_ubuntu";
	}else{
		buildnode="for_aliyun_001";
	}
	return buildnode;
}
//������������
def DropContainer(){
	try{
        sh '''docker rm -f ${applicationname}'''
    }catch(e){
        echo "��һ�ι���${e}";
    }
}

//����
def DeployApplication(){
	DropContainer();
    sh 'docker run --name ${applicationname} -p ${applicationname}:80 -d ${applicationname}'	
}
