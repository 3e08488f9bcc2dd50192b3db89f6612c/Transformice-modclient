package
{
   import flash.display.Bitmap;
   import flash.display.Sprite;
   
   public final class LoadingProgressBar extends Sprite
   {
       
      
      var MouseBitmap:Class;
      
      var loadingBar:Sprite;
      
      public function LoadingProgressBar()
      {
         this.MouseBitmap = LoadingProgressBar_MouseBitmap;
         super();
         var _loc1_:Bitmap = new this.MouseBitmap();
         _loc1_.x = 17;
         addChild(_loc1_);
         this.loadingBar = new Sprite();
         this.loadingBar.y = 375;
         addChild(this.loadingBar);
         this.Progress(0);
      }
      
      public function Progress(param1:Number) : void
      {
         this.loadingBar.graphics.clear();
         this.loadingBar.graphics.lineStyle(1,4345711,1,true);
         this.loadingBar.graphics.beginFill(6321056);
         this.loadingBar.graphics.drawRect(0,0,250,6);
         this.loadingBar.graphics.endFill();
         this.loadingBar.graphics.lineStyle(1,8498630,1,true);
         this.loadingBar.graphics.beginFill(8498630);
         this.loadingBar.graphics.drawRect(1,1,248 * param1,4);
         this.loadingBar.graphics.endFill();
      }
   }
}
