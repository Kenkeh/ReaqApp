import { Button } from "@mui/material";
import React from "react";
import Centerer from "../../../MiniTools/Centerer/Centerer";
import './LandingArea.css';


export default function LandingArea (props) {
    const nothing = () => {return;}
    return (
        <Centerer>
            <div className="landing_area_grid">
                <Button size="large" onClick={nothing}>
                    Explore
                </Button>
                <Centerer>
                    <span>or</span>
                </Centerer>
                <Centerer>
                    <span>register and/or login to manage projects</span>
                </Centerer>
            </div>
        </Centerer>
    );
}