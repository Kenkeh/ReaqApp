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



export default function App() {

  
  const topBarSize = 40;

  const [currentPage, setCurrentPage] = useState(0);
  const [currentUser,setCurrentUser] = useState(undefined);
  const [currentLogin, setCurrentLogin] = useState('');
  const [currentProject, setCurrentProject] = useState(undefined);
  const [currentProjectHasBeenEdited, setCurrentProjectHasBeenEdited] = useState(false);

  useEffect(()=>{
    var appInfo = Cookies.get('ReaqInfo');
    if (appInfo){
      appInfo = JSON.parse(appInfo);
      setCurrentUser(appInfo.user);
      setCurrentLogin(appInfo.loginSession);
      setCurrentPage(appInfo.currentPage);
      setCurrentProject(appInfo.currentProject);
    }
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
    }
  }
  const setProject = (project) =>{
    const oldInfo = Cookies.get('ReaqInfo');
    if (oldInfo){
      const appInfo = JSON.parse(oldInfo);
      Cookies.set('ReaqInfo',JSON.stringify({...appInfo, currentProject: project}));
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
  

  return (
    <BrowserRouter>
      <ThemeProvider theme={overTheme}>
        <CssBaseline />
        <AppTopBar 
          setUser={setUser} 
          user={currentUser} 
          project={currentProject}
          projectEdited={currentProjectHasBeenEdited}
          login={currentLogin}
          currentScreen={currentPage}
          switchToPage={setPage}
        />
        <AppBG topBarHeight={topBarSize}>
          <MainSwitcher
          pageNumber={currentPage}
          setPageNumber={setPage} 
          user={currentUser} 
          setUser={setUser}
          project={currentProject} 
          setProject={setProject}
          setProjectEdited = {setCurrentProjectHasBeenEdited}
          loginSession={currentLogin}  
          />
        </AppBG>
      </ThemeProvider>
    </BrowserRouter>
  );
}
