projectname="configcenter";
gitpath="https://github.com/wulinacha/ConfigCenter.git";
workpath="/home/share/configcenter";
dllpath="/home/share/configcenter/lib";
version="v1.0";

//运行应用程序相关配置
applicationname="${params.applicationname}";//应用程序名
gitbranch="${params.gitbranch}";//git分支名称
evn="${params.evn}";//应用程序运行环境 例如 Development/Staging/Production
evnlowercase="${params.evn}".toLowerCase();//应用程序运行环境,小写
myapplcationpoint="${params.applcationpoint}";//应用程序端口 
mybuildpath="${workpath}/${params.buildpath}";//执行编译目录 例如 /share/wms/src/Api/WMS.WebApi
myapplicationtype="${applicationtype}".toLowerCase();//应用程序类型 例如 Console/Web
buildnode=GetBuildNode(evn);//获取编译机器标签

/////// 编译构建（主要工作编译程序，生成镜像，将镜像推送到私有仓）
node (buildnode)
{
    stage('获取代码'){
        dir(workpath){
           git branch: gitbranch,
           url: gitpath
       }
       dir(workpath){
            sh '''git submodule init
            git submodule update'''
       }
    }
    stage('编译'){
        dir(mybuildpath){
            sh '''rm bin/publish -rf
            dotnet publish -c Release -f netcoreapp3.1 -o bin/publish
            '''
        }
    }
    stage('构建') {
        dir(mybuildpath){
            sh 'docker build -t ${applicationname} .'
        }
    }
}
///////// end

///////// 部署（主要工作是从私有仓或者本地获取镜像，通过镜像启动程序）
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

//////// 函数
//获取编译机器标签
def GetBuildNode(environmental){
	if(environmental=='Development'){
		buildnode="for_ubuntu";
	}else{
		buildnode="for_aliyun_001";
	}
	return buildnode;
}
//销毁现有容器
def DropContainer(){
	try{
        sh '''docker rm -f ${applicationname}'''
    }catch(e){
        echo "第一次构建${e}";
    }
}

//部署
def DeployApplication(){
	DropContainer();
    sh 'docker run --name ${applicationname} -p ${applicationname}:80 -d ${applicationname}'	
}
