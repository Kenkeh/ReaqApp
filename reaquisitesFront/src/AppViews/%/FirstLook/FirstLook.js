import { Button, TextField, Select, MenuItem, useAutocomplete } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import ArrowDropdownIcon from '@mui/icons-material/ArrowDropDown'
import { useEffect, useState } from "react";
import { overTheme } from '../../../overTheme';
import './FirstLook.css';
import { ServerRouteHTTPS } from '../../../AppPaths';
import ProjectCard from '../../../Elements/ProjectCard/ProjectCard';
import { getUserProject } from '../../../AppAPI';


export default function FirstLook (props) {

    const maxRecentProjectsSize = 5;
    const [showNewProject, setShowNewProject] = useState(false);
    const [newProject, setNewProject] = useState({
        name: '',
        description: '',
        template: 0
    });
    const [newProjectNameError, setNewProjectNameError] = useState('Project name cannot be empty');
    const [showRecentProjects, setShowRecentProjects] = useState(false);
    const [recentProjects, setRecentProjects] = useState([]);
    const [showFavoriteProjects, setShowFavoriteProjects] = useState(false);


    useEffect(()=>{
        var recProjects = props.user.projects;
        recProjects.sort((a,b)=>{
            if (a.lastModified > b.lastModified){
                return -1;
            }else if (a.lastModified < b.lastModified){
                return 1;
            }else{
                return 0;
            }
        });
        recProjects = recProjects.slice(0,(Math.min(maxRecentProjectsSize,recProjects.length)));
        setRecentProjects(recProjects);
    },[props.user.projects]);

    const toogleDropdown = (dpn) => (event) =>{
        switch(dpn){
            case 0:
                setShowNewProject(prev => !prev);
                break;
            case 1:
                setShowRecentProjects(prev => !prev);
                break;
            case 2:
                setShowFavoriteProjects(prev => !prev);
                break;
            default:
                break;
        }
    }


    const changeNewProjectName = (event) =>{
        const newName = event.target.value;
        if (newName==''){
            setNewProjectNameError('Project name cannot be empty');
        }else{
            var found = false;
            for (var i=0; i<props.user.projects.length; i++){
                if (props.user.projects[i].name == newName){
                    setNewProjectNameError('Project name already in use');
                    found = true;
                    break;
                }
            }
            if (!found){
                setNewProjectNameError('');
            }
        }
        setNewProject(prevState => {
            var newProj = {...prevState};
            newProj.name = newName;
            return newProj;
        });
    }
    const changeNewProjectDescripotion = (event) => {
        setNewProject(prevState => {
            var newProj = {...prevState};
            newProj.description = event.target.value;
            return newProj;
        });
    }

    const startNewProject = (event) =>{
        if (newProjectNameError) return;
        fetch(ServerRouteHTTPS+'project/'+props.user.account+'/add', {
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
            body: JSON.stringify({
                projectName: newProject.name, 
                projectDesc: newProject.description, 
                loginSession: props.loginSession
            }) // body data type must match "Content-Type" header

        }).then( res => res.json() ).then( res =>
            {
                if (res.error!==undefined){
                    console.log("Server error: "+res.message);
                }else{
                    props.setProject(res);
                    props.setUser({...props.user, projects: [...props.user.projects, {
                        name: res.name,
                        description: res.description,
                        version: res.version,
                        isPublished: res.isPublished,
                        id: res.projectId,
                        lastModified: Date.now()
                    }]});
                    props.openProjectEdition();
                }
            }).catch(err=>{
            console.log("Error treating request: "+err);
        }).catch(err=>{
            console.log("Error parsing request: "+err);
        }).catch(err=>{
            console.log("Error on request: "+err);
        });
    }
    
    const goToEditProject = (projectRef) =>{
        getUserProject(props.user.account, projectRef).then((project) =>{
            props.setProject(project);
            props.openProjectEdition();
        }).catch(err=>{
            console.log("Error treating request: "+err);
        }).catch(err=>{
            console.log("Error parsing request: "+err);
        }).catch(err=>{
            console.log("Error on request: "+err);
        });
    }

    return (
        <div className="first_look_grid">
            <Button size="large" color='primary' variant={showNewProject ? 'contained' : 'outlined'} onClick={toogleDropdown(0)} 
            style={{width: '100%', height: '100px', color: 'white', margin: '10px 5px 0px'}}>
                <AddIcon className={ showNewProject ? 'dpa_open' : 'dpa_closed'}
                style={{transition: 'transform 0.3s'}}/>
                Create New Project
            </Button>
            <div className={showNewProject ? 'fl_new_project_container fl_npc_open' : 'fl_new_project_container fl_npc_closed'} 
            style={{backgroundColor: overTheme.palette.primary.main}}>
                <div className='fl_new_project_flex'>
                    <div className='fl_new_project_flex_item fl_np_fi_first'>
                        <TextField
                            label="New project name"
                            variant="outlined"
                            color='secondary'
                            value={newProject.name}
                            onChange={changeNewProjectName}
                            style={{minWidth: 'calc(100% - 10px)'}}
                        />
                        <Select
                            variant="outlined"
                            color='secondary'
                            value={newProject.template}
                            style={{minWidth: 'calc(100% - 10px)'}}
                        >
                            <MenuItem value={0}>No Template</MenuItem>
                        </Select>
                    </div>
                    <div className='fl_new_project_flex_item'>
                        <TextField
                            label="New project description"
                            multiline={true}
                            rows={4}
                            variant="outlined"
                            color='secondary'
                            value={newProject.description}
                            onChange={changeNewProjectDescripotion}
                            style={{minWidth: '100%'}}
                        />
                    </div>
                    <div className='fl_new_project_flex_item'>
                        <Button 
                        color={ newProjectNameError=='' ? 'secondary' : 'error'} 
                        variant={newProjectNameError=='' ? 'contained' : 'outlined'}
                        disableElevation
                        onClick={ newProjectNameError=='' ? startNewProject : undefined}
                        style={{minWidth: '100%',minHeight: '100%'}}>
                            { newProjectNameError=='' ? 'Create' : newProjectNameError}   
                        </Button>
                    </div>
                </div>
            </div>
            <Button size="large" color='primary' variant={showRecentProjects ? 'contained' : 'outlined'} onClick={toogleDropdown(1)}
            style={{width: '100%', height: '100px', color: 'white', margin: '5px 5px 0px'}}>
                <ArrowDropdownIcon className={ showRecentProjects ? 'dpa_open' : 'dpa_closed'}
                style={{transition: 'transform 0.3s', MozTransition: 'transform 0.3s', OTransition: 'transform 0.3s', WebkitTransition: 'transform 0.3s'}}/>
                Recent Projects
            </Button>
            <div className={showRecentProjects ? 'fl_new_project_container fl_npc_open' : 'fl_new_project_container fl_npc_closed'} 
            style={{backgroundColor: overTheme.palette.primary.main}}>
                <div className='fl_recent_projects_flex'>
                    {recentProjects.map((project, index)=>{
                        return  <div key={index} className='fl_rp_card_container' style={{gridColumn: index+1}}>
                                    <ProjectCard textSize={0} project={project} editClick = {() => goToEditProject(project.projectId)}/>
                                </div>
                    })}
                </div>
            </div>
            <Button size="large" color='primary' variant={showFavoriteProjects ? 'contained' : 'outlined'} onClick={toogleDropdown(2)}
            style={{width: '100%', height: '100px', color: 'white', margin: '5px 5px 0px'}}>
                <ArrowDropdownIcon className={ showFavoriteProjects ? 'dpa_open' : 'dpa_closed'}
                style={{transition: 'transform 0.3s', MozTransition: 'transform 0.3s', OTransition: 'transform 0.3s', WebkitTransition: 'transform 0.3s'}}/>
                Favorite Projects
            </Button>
        </div>
    );
}