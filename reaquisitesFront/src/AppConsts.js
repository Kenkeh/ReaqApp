import AddTaskTwoToneIcon from '@mui/icons-material/AddTaskTwoTone';
import TaskTwoToneIcon from '@mui/icons-material/TaskTwoTone';
import TaskAltTwoToneIcon from '@mui/icons-material/TaskAltTwoTone';
import AssignmentTwoToneIcon from '@mui/icons-material/AssignmentTwoTone';
import PlaylistAddCheckTwoToneIcon from '@mui/icons-material/PlaylistAddCheckTwoTone';
import AssignmentIndTwoToneIcon from '@mui/icons-material/AssignmentIndTwoTone';
import AssignmentReturnedTwoToneIcon from '@mui/icons-material/AssignmentReturnedTwoTone';
import AssignmentTurnedInTwoToneIcon from '@mui/icons-material/AssignmentTurnedInTwoTone';
import AssignmentLateTwoToneIcon from '@mui/icons-material/AssignmentLateTwoTone';
import FormatListNumberedTwoToneIcon from '@mui/icons-material/FormatListNumberedTwoTone';
import ListAltTwoToneIcon from '@mui/icons-material/ListAltTwoTone';
import ErrorTwoToneIcon from '@mui/icons-material/ErrorTwoTone';
import BrokenImageTwoToneIcon from '@mui/icons-material/BrokenImageTwoTone';
import ThumbUpTwoToneIcon from '@mui/icons-material/ThumbUpTwoTone';
import ClassTwoToneIcon from '@mui/icons-material/ClassTwoTone';
import AccountTreeTwoToneIcon from '@mui/icons-material/AccountTreeTwoTone';
import AccessTimeTwoToneIcon from '@mui/icons-material/AccessTimeTwoTone';
import TimelapseTwoToneIcon from '@mui/icons-material/TimelapseTwoTone';
import SentimentVerySatisfiedTwoToneIcon from '@mui/icons-material/SentimentVerySatisfiedTwoTone';
import SentimentVeryDissatisfiedTwoToneIcon from '@mui/icons-material/SentimentVeryDissatisfiedTwoTone';
import AssuredWorkloadTwoToneIcon from '@mui/icons-material/AssuredWorkloadTwoTone';
import GroupWorkTwoToneIcon from '@mui/icons-material/GroupWorkTwoTone';
import WorkspacesTwoToneIcon from '@mui/icons-material/WorkspacesTwoTone';
import WorkspacePremiumTwoToneIcon from '@mui/icons-material/WorkspacePremiumTwoTone';
import WorkTwoToneIcon from '@mui/icons-material/WorkTwoTone';
import WorkOffTwoToneIcon from '@mui/icons-material/WorkOffTwoTone';

export const ArtefactIcons = [
    <TaskTwoToneIcon/>,
    <AddTaskTwoToneIcon/>,
    <TaskAltTwoToneIcon/>,
    <AssignmentTwoToneIcon/>,
    <PlaylistAddCheckTwoToneIcon/>,
    <AssignmentIndTwoToneIcon/>,
    <AssignmentReturnedTwoToneIcon/>,
    <AssignmentTurnedInTwoToneIcon/>,
    <AssignmentLateTwoToneIcon/>,
    <FormatListNumberedTwoToneIcon/>,
    <ListAltTwoToneIcon/>,
    <ErrorTwoToneIcon/>,
    <BrokenImageTwoToneIcon/>,
    <ThumbUpTwoToneIcon/>,
    <ClassTwoToneIcon/>,
    <AccountTreeTwoToneIcon/>,
    <AccessTimeTwoToneIcon/>,
    <TimelapseTwoToneIcon/>,
    <SentimentVerySatisfiedTwoToneIcon/>,
    <SentimentVeryDissatisfiedTwoToneIcon/>,
    <AssuredWorkloadTwoToneIcon/>,
    <GroupWorkTwoToneIcon/>,
    <WorkspacesTwoToneIcon/>,
    <WorkspacePremiumTwoToneIcon/>,
    <WorkTwoToneIcon/>,
    <GroupWorkTwoToneIcon/>,
    <WorkOffTwoToneIcon/>
]

export const currentDate = () =>{
    var date = new Date();
    var day = date.getDate();       // yields date
    var month = date.getMonth() + 1;    // yields month (add one as '.getMonth()' is zero indexed)
    var year = date.getFullYear();  // yields year
    var hour = date.getHours();     // yields hours 
    var minute = date.getMinutes(); // yields minutes
    var second = date.getSeconds(); // yields seconds

    if (day<10){
        day = '0'+day;
    }
    if (month<10){
        month = '0'+month;
    }
    if (hour<10){
        hour = '0'+hour;
    }
    if (minute<10){
        minute = '0'+minute;
    }
    if (second<10){
        second = '0'+second;
    }
    // After this construct a string with the above results as below
    return year + "-" + month + "-" + day + "T" + hour + ':' + minute + ':' + second;
}

export const cytoscapeArrowHeads = [ 
    'triangle', 'chevron', 'circle', 'circle-triangle', 'diamond', 'square', 'tee',
    'triangle-backcurve', 'triangle-tee', 'triangle-cross', 'vee'
]