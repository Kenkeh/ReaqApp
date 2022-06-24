import './Login.css';
import {useState, useEffect} from 'react';
import Centerer from '../../MiniTools/Centerer/Centerer';
import Spinner from '../../MiniTools/Spinner/Spinner';
import  {overTheme} from '../../overTheme';
import { Button, TextField, Chip } from '@mui/material';
import logo from '../../Elements/images/logo192.png';
import { userAuth, userProjectList } from './../../AppAPI';
import { AppName } from '../../AppConsts';





export default function Login(props) {

  const [logginName, setLogginName] = useState('');
  const [logginNameError, setLogginNameError] = useState(false);
  const [logginNameErrorText, setLogginNameErrorText] = useState('');
  const [password, setPassword] = useState('');
  const [passwordError, setPasswordError] = useState(false);
  const [passwordErrorText, setPasswordErrorText] = useState('');
  const [logginPhase, setLogginPhase] = useState(0);
  const [logginPhaseText, setLogginPhaseText] = useState('');
  const [serverError, setServerError] = useState(false);


  useEffect(()=>{
    props.resetRef.current = resetLogin;
  },[]);


  const handleInfoChange = (info) => (event) => {
    switch (info){
      //logginName
      case 'logname':
        if (event.target.value===''){
          setLogginNameError(true);
          setLogginNameErrorText('Logging name field cannot be empty');
        }else{
          setLogginNameError(false);
          setLogginNameErrorText('');
        }
        setLogginName(event.target.value);
        break;

      //password check
      case 'password':
        if (event.target.value===''){
          setPasswordError(true);
          setPasswordErrorText('Password field cannot be empty');
        }else if (event.target.value.length<8){
          setPasswordError(true);
          setPasswordErrorText('Password must be at least 8 characters long');
        }else{
          setPasswordError(false);
          setPasswordErrorText('');
        }
        setPassword(event.target.value);
        break;
      default:
        break;
    }
  };


  const startLoginProcess = () => {

    setLogginPhase(1);
    setLogginPhaseText('Checking credentials...');

    userAuth({logName: logginName, logPass: password}).then( res => {
      if (res.error){
        setLogginPhase(0);
        setPasswordError(true);
        setLogginNameErrorText(res.message);
      }else{
        setLogginPhase(2);
        props.setUser(res);
        userProjectList(res.account).then(projectList =>{
          props.setProjectsList(projectList);
          props.goToPage(1);
        }).catch(err =>{
          setLogginPhaseText('There was an error parsing server response...');
          setServerError(true);
        })
        .catch(err =>{
          setLogginPhaseText('There was an error connecting to the server...');
          setServerError(true);
        });
      }
    }
    ).catch(err =>{
      setLogginPhaseText('There was an error parsing server response...');
      setServerError(true);
    })
    .catch(err =>{
      setLogginPhaseText('There was an error connecting to the server...');
      setServerError(true);
    });

    
  }




  const handleLogin = () => {
    if (logginNameError || passwordError) return;
    var firstUseError = false;
    if (logginName===''){
      setLogginNameError(true);
      firstUseError = true;
    }
    if (password===''){
      setPasswordError(true);
      firstUseError = true;
    }
    if (firstUseError) return;
    startLoginProcess();
  }

  const resetLogin = () =>{
    setLogginName('');
    setLogginNameError(false);
    setLogginNameErrorText('');
    setPassword('');
    setPasswordError(false);
    setPasswordErrorText('');
    setLogginPhase(0);
    setLogginPhaseText('');
    setServerError(false);
  }
  
  const title = "Log into "+AppName+" account";

  return (
    <div className={props.className}>
      <div className="login_content_container" style={{backgroundColor: overTheme.palette.primary.light}}>

        <div className={logginPhase===0 ? "login_form" : "login_form hidden"} >
          <div className="login_form_title">
            <Centerer> <span>{title}</span> </Centerer>
          </div>

          <div className="login_form_field_container">
            <Centerer>
              <TextField
                label="Account Name or E-Mail"
                variant="standard"
                value={logginName}
                onChange={handleInfoChange('logname')}
                error={logginNameError}
                helperText= {logginNameErrorText}
                style={{width: '400px'}}
              />
            </Centerer>
          </div>

          <div className="login_form_field_container">
            <Centerer>
              <TextField variant="standard" label="Password"
                    type={'password'}
                    value={password}
                    onChange={handleInfoChange('password')}
                    error={passwordError}
                    helperText= {passwordErrorText}
                    style={{width: '400px'}}
                  />
            </Centerer>
          </div>

          <div>
          </div>

          <div className="login_form_field_container">
            <Centerer>
              <Button variant="contained" disableElevation onClick={handleLogin} style={{width: '300px'}}>
                LOGIN
              </Button>
            </Centerer>
          </div>

        </div>

        <div className={logginPhase===1 ? "login_check_loading_container" : "login_check_loading_container hidden"}>
          <div className="login_check_loading">
            <Spinner msCycle="1000ms" style={{position: 'absolute'}}>
              <Centerer>
                <img className="login_check_loading_img" src={logo} alt="Here it was the logo."></img>
              </Centerer>
            </Spinner>
          </div>
          <div className='login_check_grid_container'>
            <div className='login_check_message'>
              <Centerer>
                <Chip label={logginPhaseText}/>
              </Centerer>
            </div>
            <div className={serverError ? 'login_check_button' : 'login_check_button hidden'}>
              <Centerer>
                <Button size="large" onClick={startLoginProcess}>
                  RETRY
                </Button>
              </Centerer>
            </div>
          </div>
        </div>

        <div className={logginPhase===2 ? "login_check_loading_container" : "login_check_loading_container hidden"}>
          <div className="login_complete_grid">
            <Spinner msCycle="10000ms" left>
              <Centerer>
                <img className="login_sent_img" src={logo} alt="Here it was the logo."></img>
              </Centerer>
            </Spinner>
            <div>
            </div>
            <Centerer>
              <Chip label="Login completed!"/>
            </Centerer>
            <Centerer>
              <Chip label="You can now freely use the app."/>
            </Centerer>
            <div>
            </div>
            <Spinner msCycle="10000ms">
              <Centerer>
                <img className="login_sent_img" src={logo} alt="Here it was the logo."></img>
              </Centerer>
            </Spinner>
          </div>
        </div>
      </div>
    </div>
  );
};

