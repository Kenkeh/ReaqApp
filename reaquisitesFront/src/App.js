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

  const [currentUser,setCurrentUser] = useState(undefined);
  const [currentLogin, setCurrentLogin] = useState('');

  useEffect(()=>{
    var login = Cookies.get('ReaqLoginInfo');
    if (login){
      login = JSON.parse(login);
      setCurrentUser(login.user);
      setCurrentLogin(login.loginSession);
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
      Cookies.set('ReaqLoginInfo',JSON.stringify({user: userObject, loginSession: user.loginSession}));
      setCurrentUser(userObject);
      setCurrentLogin(user.loginSession);
    }else{
      if (Cookies.get('ReaqLoginInfo')){
        Cookies.remove('ReaqLoginInfo');
      }
      setCurrentUser(undefined);
      setCurrentLogin('');
    }
  }
  

  return (
    <BrowserRouter>
      <ThemeProvider theme={overTheme}>
        <CssBaseline />
        <AppTopBar setUser={setUser} user={currentUser}/>
        <AppBG>
          <MainSwitcher user={currentUser} loginSession={currentLogin} topBarHeight={topBarSize}/>
        </AppBG>
      </ThemeProvider>
    </BrowserRouter>
  );
}
