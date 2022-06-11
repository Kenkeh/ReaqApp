import './Register.css';
import {useEffect, useState} from 'react';
import Centerer from '../../MiniTools/Centerer/Centerer';
import Spinner from '../../MiniTools/Spinner/Spinner';
import  {overTheme} from '../../overTheme';
import { Button, TextField, Chip } from '@mui/material';
import logo from '../../Elements/images/logo192.png';
import {ServerRouteHTTPS, ServerRouteHTTP, AppName} from '../../AppPaths';





export default function Register(props) {

  const [accName, setAccName] = useState('');
  const [accNameError, setAccNameError] = useState(false);
  const [accNameErrorText, setAccNameErrorText] = useState('');
  const [userName, setUserName] = useState('');
  const [userNameError, setUserNameError] = useState(false);
  const [userNameErrorText, setUserNameErrorText] = useState('');
  const [eMail, setEmail] = useState('');
  const [eMailError, setEmailError] = useState(false);
  const [eMailErrorText, setEmailErrorText] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [password, setPassword] = useState('');
  const [passwordError, setPasswordError] = useState(false);
  const [passwordErrorText, setPasswordErrorText] = useState('');
  const [registrationPhase, setRegistrationPhase] = useState(0);
  const [registrationPhaseText, setRegistrationPhaseText] = useState('');
  const [serverError, setServerError] = useState(false);


  const emailER = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

  useEffect(()=>{
    props.resetRef.current = resetRegister;
  },[]);


  const handleInfoChange = (info) => (event) => {
    switch (info){
      //email check
      case 'email':
        if (event.target.value===''){
          setEmailError(true);
          setEmailErrorText('E-mail field cannot be empty');
        }
        else if (!emailER.test(event.target.value)){
          setEmailError(true);
          setEmailErrorText('Invalid e-mail');
        }else{
          setEmailError(false);
          setEmailErrorText('');
        }
        setEmail(event.target.value);
        break;

      //accname check
      case 'accname':
        if (event.target.value===''){
          setAccNameError(true);
          setAccNameErrorText('Account Name field cannot be empty');
        }else{
          setAccNameError(false);
          setAccNameErrorText('');
        }
        setAccName(event.target.value);
        break;
    
      //nickname check
      case 'nickname':
        if (event.target.value===''){
          setUserNameError(true);
          setUserNameErrorText('NickName field cannot be empty');
        }else{
          setUserNameError(false);
          setUserNameErrorText('');
        }
        setUserName(event.target.value);
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
  
  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };
  
  const handleMouseDownPassword = (event) => {
    event.preventDefault();
  };


  const startRegisterProcess = () => {

    setRegistrationPhase(1);
    setRegistrationPhaseText('Checking credentials avaliability...');

    //start registration verification
    fetch(ServerRouteHTTPS+'user')
    .then(res => {
      if (res.ok){
        res.json().then(users => {
          //1. Check credentials unique

          for (var i=0;i<users.length;i++){
            if (users[i].nick===userName){
              setUserNameError(true);
              setUserNameErrorText('Username already in use.');
            }
            if (users[i].eMail===eMail){
              setEmailError(true);
              setEmailErrorText('E-mail already in use.');
            }
            if (users[i].account===accName){
              setAccNameError(true);
              setAccNameErrorText('Account name already in use.');
            }
            if (userNameError && eMailError && accNameError){ // CHECK FOR ALL THE ERRORS TO MAKE THE USER KNOW
              setRegistrationPhase(0);
              return;
            }
          }

          if (userNameError || eMailError || accNameError){
            setRegistrationPhase(0);
            return;
          }
          
          setRegistrationPhaseText('Sending verification link...');

          fetch(ServerRouteHTTPS+'user/emailver', {
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            mode: 'cors', // no-cors, *cors, same-origin
            cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
            credentials: 'same-origin', // include, *same-origin, omit
            headers: {
              'Content-Type': 'application/json'
              // 'Content-Type': 'application/x-www-form-urlencoded',
            },
            redirect: 'follow', // manual, *follow, error
            referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
            body: JSON.stringify({userName: userName, userAccount: accName, userEmail: eMail, userPassword: password}) // body data type must match "Content-Type" header
          })
          .then(res => {
            if (res.ok){

              //Everything OK
              setRegistrationPhase(2);

            }else{
              setRegistrationPhaseText('There was an error connecting to the server...');
              setServerError(true);
            }
          })
          .catch(() => {
            setRegistrationPhaseText('There was an error connecting to the server...');
            setServerError(true);
          });
        });
      }else{
        setRegistrationPhaseText('There was an error connecting to the server...');
        setServerError(true);
      }
    })
    .catch(() => {
      setRegistrationPhaseText('There was an error connecting to the server...');
      setServerError(true);
    });
  }




  const handleRegister = () => {
    if (userNameError || passwordError || eMailError || accNameError) return;
    var firstUseError = false;
    if (userName===''){
      setUserNameError(true);
      firstUseError = true;
    }
    if (password===''){
      setPasswordError(true);
      firstUseError = true;
    }
    if (eMail===''){
      setEmailError(true)
      firstUseError = true;
    }
    if (accName===''){
      setAccNameError(true)
      firstUseError = true;
    }
    if (firstUseError) return;
    startRegisterProcess();
  }

  const resetRegister = () =>{
    setAccName('');
    setAccNameError(false);
    setAccNameErrorText('');
    setUserName('');
    setUserNameError(false);
    setUserNameErrorText('');
    setEmail('');
    setEmailError(false);
    setEmailErrorText('');
    setShowPassword(false);
    setPassword('');
    setPasswordError(false);
    setPasswordErrorText('');
    setRegistrationPhase(0);
    setRegistrationPhaseText('');
    setServerError(false);
  }
  
  const title = "Register "+AppName+" account";

  return (
    <div className={props.className}>
      <div className="register_content_container" style={{backgroundColor: overTheme.palette.primary.light}}>

        <div className={registrationPhase===0 ? "register_form" : "register_form hidden"} >
          <div className="register_form_title">
            <Centerer> <span>{title}</span> </Centerer>
          </div>

          <div className="register_form_field_container">
            <Centerer>
              <TextField
                label="Account Name"
                variant="standard"
                value={accName}
                onChange={handleInfoChange('accname')}
                error={accNameError}
                helperText= {accNameErrorText}
                style={{width: '400px'}}
              />
            </Centerer>
          </div>

          <div className="register_form_field_container">
            <Centerer>
              <TextField
                label="NickName"
                variant="standard"
                value={userName}
                onChange={handleInfoChange('nickname')}
                error={userNameError}
                helperText= {userNameErrorText}
                style={{width: '400px'}}
              />
            </Centerer>
          </div>

          <div className="register_form_field_container">
            <Centerer>
            <TextField variant="standard" label="Password"
                  type={showPassword ? 'text' : 'password'}
                  value={password}
                  onChange={handleInfoChange('password')}
                  error={passwordError}
                  helperText= {passwordErrorText}
                  style={{width: '400px'}}
                />
            </Centerer>
          </div>

          <div className="register_form_field_container">
            <Centerer>
              <TextField
              label="E-mail"
              variant="standard"
              onChange={handleInfoChange('email')}
              value={eMail}
              helperText={eMailErrorText}
              error={eMailError}
              style={{width: '400px'}}
              />
            </Centerer>
          </div>
          
          <div className="register_form_field_container">
            <Centerer>
              <span>Regístrate aquí bajo tu cuenta y riesgo.</span>
            </Centerer>
          </div>

          <div className="register_form_field_container">
            <Centerer>
              <Button variant="contained" disableElevation onClick={handleRegister} style={{width: '300px'}}>
                REGISTER
              </Button>
            </Centerer>
          </div>

        </div>

        <div className={registrationPhase===1 ? "register_check_loading_container" : "register_check_loading_container hidden"}>
          <div className="register_check_loading_img_container">
            <Spinner msCycle="1000ms" style={{position: 'absolute'}}>
              <Centerer>
                <img className="register_check_loading_img" src={logo} alt="Here it was the logo."></img>
              </Centerer>
            </Spinner>
          </div>
          <div className='register_check_grid_container'>
            <div className='register_check_message'>
              <Centerer>
                <Chip label={registrationPhaseText}/>
              </Centerer>
            </div>
            <div className={serverError ? 'register_check_button' : 'register_check_button hidden'}>
              <Centerer>
                <Button size="large" onClick={startRegisterProcess}>
                  RETRY
                </Button>
              </Centerer>
            </div>
          </div>
        </div>

        <div className={registrationPhase===2 ? "register_check_loading_container" : "register_check_loading_container hidden"}>
          <div className="register_complete_grid">
            <Spinner msCycle="10000ms" left>
              <Centerer>
                <img className="register_sent_img" src={logo} alt="Here it was the logo."></img>
              </Centerer>
            </Spinner>
            <Centerer>
              <Chip label="Registration almost done!"/>
            </Centerer>
            <Centerer>
              <Chip label="We have sent you an e-mail to your direction to verify and complete register."/>
            </Centerer>
            <Centerer>
              <Button size="large" onClick={startRegisterProcess}>
                RESEND
              </Button>
            </Centerer>
            <Spinner msCycle="10000ms">
              <Centerer>
                <img className="register_sent_img" src={logo} alt="Here it was the logo."></img>
              </Centerer>
            </Spinner>
          </div>
        </div>
      </div>
    </div>
  );
};

