import { Routes, Route } from 'react-router-dom';
import React, { useEffect, useState } from 'react';
import AccIcon from '../Elements/images/green_valid.png';
import RejIcon from '../Elements/images/red_cancel.png';
import RedirectionPage from './RedirectionPage/RedirectionPage';
import LandingArea from './1/LandingArea/LandingArea';
import FirstLook from './1/FirstLook/FirstLook';
import ProjecEdit from './2/ProjectEdit/ProjectEdit';



export default function MainSwitcher(props) {

  const currentPage = (number) =>{
    switch (number){
      case 0:
        return <LandingArea/>
      case 1:
        return  <FirstLook 
                  account={props.userAccount}
                  projectList={props.userProjectsPreview}
                  setProjectList={props.setProjecPreviewList}
                  //loginSession={props.loginSession} 
                  setProject={props.setProject} 
                  currentProject={props.project}
                  openProjectEdition={() => props.setPageNumber(2)}
                />
      case 2: 
        return <ProjecEdit
                activeProject = {props.project}
                setActiveProject = {props.setProject}
                setActiveProjectModified = {props.setProjectEdited}
              />
    }
  }

  return (
    <Routes>
      <Route path="/" element={currentPage(props.pageNumber)}
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
      {/* was a nice try but it complicates things out
        editRoutes &&
        editRoutes.map( (proj, index) => 
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
        */
      }
    </Routes>
  );
};
