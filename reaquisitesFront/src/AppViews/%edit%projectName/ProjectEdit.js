import { useEffect } from 'react';
import './ProjectEdit.css';
import { getUserProject } from '../../AppAPI';


export default function ProjectEdit (props) {

    useEffect( () => {
        if (!props.activeProject || props.editingProject.name != props.activeProject.name){
            const projectName = props.editingProject.name.replaceAll(' ','_');
            getUserProject(props.user.account,projectName).then(project =>{
                if (project.error){
                    console.log(project.message);
                }else{
                    props.setActiveProject(project);
                }
            }).catch(err=>{
                //parsing error
            }).catch(err =>{
                //server error
            });
        }
    },[]);
    return ("HAHA SALUDOS");
    
}