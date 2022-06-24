import Cookies from 'js-cookie';

import './App.css';
import {useEffect, useState} from 'react';
import CssBaseline from '@mui/material/CssBaseline';
import AppTopBar from './AppTopBar/AppTopBar';
import AppBG from './Elements/AppBG/AppBG';
import { ThemeProvider } from '@mui/material';
import { overTheme } from './overTheme';
import MainSwitcher from './AppViews/MainSwitcher';
import { BrowserRouter } from 'react-router-dom';
import { getUserProject, saveProject, userProjectList } from './AppAPI';
import LoadingFrame from './LoadingFrame/LoadingFrame';
import { AppName } from './AppConsts';



export default function App() {

  
  const topBarSize = 40;

  const [currentPage, setCurrentPage] = useState(0);
  const [currentUser,setCurrentUser] = useState(undefined);
  const [currentLogin, setCurrentLogin] = useState('');
  const [currentUserProjectList, setCurrentUserProjectList] = useState(undefined);
  const [currentProject, setCurrentProject] = useState(undefined);
  const [currentProjectId, setCurrentProjectId] = useState(-1);
  const [currentProjectHasBeenEdited, setCurrentProjectHasBeenEdited] = useState(false);
  const [loadingInfo, setLoadingInfo] = useState(2);


  useEffect(()=>{
    var appInfo = Cookies.get('ReaqInfo');
    if (appInfo){
      appInfo = JSON.parse(appInfo);
      setCurrentUser(appInfo.user);

      userProjectList(appInfo.user.account).then(projectList =>{
        setCurrentUserProjectList(projectList);
        setLoadingInfo(prev => prev-1);
      }).catch(err =>{
        //parse error
      })
      .catch(err =>{
        //connection error
      });

      setCurrentLogin(appInfo.loginSession);
      setCurrentPage(appInfo.currentPage);
      if (appInfo.currentProject>=0){
        getUserProject(appInfo.user.account,appInfo.currentProject).then(project =>{
          setCurrentProject(project);
          setLoadingInfo(prev => prev-1);
        }).catch(err =>{
          //parse error
        })
        .catch(err =>{
          //connection error
        });
      }
      setCurrentProjectId(appInfo.currentProject);
    }else{
      setLoadingInfo(0);
    }
    document.title=AppName;
  },[]);

  const setUser = (user) =>{
    if (user){
      var userObject = {
        nick: user.nick,
        account: user.account,
        eMail: user.eMail,
        registerDate: user.registerDate,
        projects: user.projects
      }
      const oldInfo = Cookies.get('ReaqInfo');
      if (oldInfo){
        const appInfo = JSON.parse(oldInfo);
        Cookies.set('ReaqInfo',JSON.stringify({...appInfo, user: userObject, loginSession: user.loginSession}));
      }else{
        Cookies.set('ReaqInfo',JSON.stringify({currentPage: 1, currentProject: undefined, user: userObject, loginSession: user.loginSession}));
      }
      setCurrentUser(userObject);
      setCurrentLogin(user.loginSession);
    }else{
      if (Cookies.get('ReaqInfo')){
        Cookies.remove('ReaqInfo');
      }
      setCurrentUser(undefined);
      setCurrentLogin('');
      setCurrentPage(0);
      setCurrentProject(undefined);
      setCurrentProjectId(-1);
      setCurrentUserProjectList(undefined);
    }
  }
  const setProject = (project) =>{
    const oldInfo = Cookies.get('ReaqInfo');
    if (oldInfo){
      const appInfo = JSON.parse(oldInfo);
      Cookies.set('ReaqInfo',JSON.stringify({...appInfo, currentProject: project.projectId}));
    }else{
      //ERROR
    }
    setCurrentProject(project);
  }
  const setPage = (number) =>{
    const oldInfo = Cookies.get('ReaqInfo');
    if (oldInfo){
      const appInfo = JSON.parse(oldInfo);
      Cookies.set('ReaqInfo', JSON.stringify({...appInfo, currentPage: number}));
    }else{
      //ERROR
    }
    setCurrentPage(number);
  }
  const saveCurrentProject = () =>{
    setLoadingInfo(1);
    saveProject(currentUser.account, currentProject).then(res =>{
      setLoadingInfo(0);
      setCurrentProjectHasBeenEdited(false);
    }).catch(err=>{
      //server error
    });
  }
  

  return (
    <BrowserRouter>
      <ThemeProvider theme={overTheme}>
        <CssBaseline />
        <AppTopBar 
          setUser={setUser}
          setProjectsList={setCurrentUserProjectList} 
          user={currentUser} 
          project={currentProject}
          projectEdited={currentProjectHasBeenEdited}
          //login={currentLogin}
          currentScreen={currentPage}
          switchToPage={setPage}
          saveProject={saveCurrentProject}
        />
        <AppBG topBarHeight={topBarSize}>
          <MainSwitcher
            pageNumber={currentPage}
            setPageNumber={setPage} 
            userAccount={currentUser ? currentUser.account : undefined}
            userProjectsPreview={currentUserProjectList}
            setProjecPreviewList={setCurrentUserProjectList}
            project={currentProject}
            setProject={setProject}
            setProjectEdited = {setCurrentProjectHasBeenEdited}
            //loginSession={currentLogin}  
          />
        </AppBG>
        {loadingInfo>0 && <LoadingFrame topBarHeight={topBarSize}/>}
      </ThemeProvider>
    </BrowserRouter>
  );
}
