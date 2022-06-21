
import Spinner from '../MiniTools/Spinner/Spinner';
import './LoadingFrame.css';
import logo from '../Elements/images/logo192.png';
import Centerer from '../MiniTools/Centerer/Centerer';
import { overTheme } from '../overTheme';



export default function LoadingFrame(props) {

  
  
  return (
    <div className='loadingFrameContainer' style={{backgroundColor: overTheme.palette.primary.main, height: 'calc(100% - '+props.topBarHeight+'px)'}}>
      <Centerer>
        <div>
          <Spinner msCycle="1000ms" style={{position: 'absolute'}}>
            <Centerer>
              <img className="login_check_loading_img" src={logo} alt="Here it was the logo."></img>
            </Centerer>
          </Spinner>
        </div>
      </Centerer>
    </div>
  );
}
