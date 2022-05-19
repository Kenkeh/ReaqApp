
using Microsoft.AspNetCore.Mvc;
public class ARFactory {
    public static JsonResult createJSONErrorResult(int id){
        return new JsonResult(new { error = id});
    }
    public static JsonResult createJSONMessageResult(string msg){
        return new JsonResult(new { message = msg});
    }
}