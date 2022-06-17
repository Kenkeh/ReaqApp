import { Routes, Route } from 'react-router-dom';
import React from 'react';
import AccIcon from '../Elements/images/green_valid.png';
import RejIcon from '../Elements/images/red_cancel.png';
import RedirectionPage from './RedirectionPage/RedirectionPage';
import LandingArea from './%/LandingArea/LandingArea';
import FirstLook from './%/FirstLook/FirstLook';
import ProjecEdit from './%edit%projectName/ProjectEdit/ProjectEdit';



export default function MainSwitcher(props) {
  return (
    <Routes>
      <Route path="/" element={
        props.user ? 
        <FirstLook 
        user={props.user} 
        loginSession={props.loginSession} 
        setProject={props.setProject} 
        currentProject={props.project}/> :
        <LandingArea/>
      }
      />
      <Route path="/register-exp" element={
        <RedirectionPage
          message="Ups! It seems that link has expired..." 
          icon={RejIcon}/>
        } 
      />
      <Route path="/register-acc" element={
        <RedirectionPage
          message="Congratulations, your register is completed! You can now log in!"
          icon={AccIcon}
        />} 
      />
      {
        props.user &&
        props.user.projects.map( (proj, index) => 
          <Route key={index} path={"/edit/"+proj.id} element={
            <ProjecEdit
              user = {props.user} 
              editingProject = {proj}
              activeProject = {props.project}
              setActiveProject = {props.setProject}
              setActiveProjectModified = {props.setProjectEdited}
            />
          }/>
        )
      }
    </Routes>
  );
};
