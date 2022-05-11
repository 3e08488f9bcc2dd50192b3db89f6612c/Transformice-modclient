package
{
   import flash.display.Loader;
   import flash.display.LoaderInfo;
   import flash.display.Shape;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.events.KeyboardEvent;
   import flash.events.ProgressEvent;
   import flash.events.TimerEvent;
   import flash.external.ExternalInterface;
   import flash.net.URLRequest;
   import flash.system.ApplicationDomain;
   import flash.system.Security;
   import flash.utils.*;
   
   public class Main extends Sprite
   {
       
      
      var ProgressBar:LoadingProgressBar;
      
      var tfmLoader:Loader;
      
      var tfm:Object;
      
      var interfaceHidden:Boolean;
      
      var originalAlphaValues:Dictionary;
      
      var gameOutline:Shape;
      
      public function Main()
      {
         super();
         if(stage)
         {
            this.init();
         }
         else
         {
            addEventListener(Event.ADDED_TO_STAGE,this.init);
         }
      }
      
      function init(param1:Event = null) : void
      {
         removeEventListener(Event.ADDED_TO_STAGE,this.init);
         stage.align = "B";
         stage.scaleMode = "noScale";
         stage.quality = "high";
         if(ExternalInterface.available)
         {
            ExternalInterface.addCallback("AnalyzeSWF",this.AnalyzeSWF);
            ExternalInterface.addCallback("SetAlignment",this.SetAlignment);
            ExternalInterface.addCallback("SetZoom",this.SetZoom);
            ExternalInterface.addCallback("SetQuality",this.SetQuality);
         }
         this.ProgressBar = new LoadingProgressBar();
         this.ProgressBar.x = 275;
         this.ProgressBar.y = 75;
         addChild(this.ProgressBar);
         this.tfmLoader = new Loader();
         this.tfmLoader.mouseEnabled = false;
         Security.allowDomain("*");
         this.tfmLoader.contentLoaderInfo.addEventListener(Event.COMPLETE,this.LoadComplete);
         this.tfmLoader.contentLoaderInfo.addEventListener(ProgressEvent.PROGRESS,this.LoadProgress);
         addChild(this.tfmLoader);
         var _loc2_:Date = new Date();
         this.tfmLoader.load(new URLRequest("http://www.transformice.com/Transformice.swf?n=" + _loc2_.time));
      }
      
      function LoadProgress(param1:ProgressEvent) : void
      {
         this.ProgressBar.Progress(param1.bytesLoaded / param1.bytesTotal);
      }
      
      function LoadComplete(param1:Event) : void
      {
         removeChild(this.ProgressBar);
         this.tfmLoader.contentLoaderInfo.removeEventListener(Event.COMPLETE,this.LoadComplete);
         this.tfmLoader.contentLoaderInfo.removeEventListener(ProgressEvent.PROGRESS,this.LoadProgress);
      }
      
      function Analyze(param1:TimerEvent) : void
      {
         this.AnalyzeSWF();
      }
      
      function AnalyzeSWF() : void
      {
         var variable:XML = null;
         var appDomainClassNames:Vector.<String> = null;
         var className:String = null;
         var varName:String = null;
         var declaredType:String = null;
         var instanceType:String = null;
         var encryptionKey:Array = null;
         var gameClass:Class = null;
         var classXML:XML = null;
         var methodName:String = null;
         var key:Vector.<int> = null;
         var contentLoaderInfo:LoaderInfo = ((this.tfmLoader.getChildAt(0) as Sprite).getChildAt(0) as Loader).contentLoaderInfo as LoaderInfo;
         var applicationDomain:ApplicationDomain = contentLoaderInfo.applicationDomain;
         this.tfm = contentLoaderInfo.content;
         if(this.tfm.stage)
         {
            this.initTfm();
         }
         else
         {
            this.tfm.addEventListener(Event.ADDED_TO_STAGE,this.initTfm);
         }
         var tfmXML:XML = describeType(this.tfm);
         var tfmVariables:XMLList = tfmXML.variable;
         for each(variable in tfmVariables)
         {
            varName = variable.@name;
            declaredType = variable.@type;
            instanceType = describeType(this.tfm[varName]).@name;
            if(declaredType == "Object" && instanceType == "Array")
            {
               encryptionKey = this.tfm[varName];
               if(ExternalInterface.available)
               {
                  ExternalInterface.call("EncryptionKey",encryptionKey.toString());
               }
               break;
            }
         }
         appDomainClassNames = applicationDomain.getQualifiedDefinitionNames();
         for each(className in appDomainClassNames)
         {
            try
            {
               gameClass = applicationDomain.getDefinition(className) as Class;
               classXML = describeType(gameClass);
               if(classXML.variable.length() >= 1 && classXML.variable.(@type == "flash.utils::Dictionary").length() == 1 && classXML.method.length() == 2 && classXML.method.(@returnType == "__AS3__.vec::Vector.<int>").length() == 2)
               {
                  methodName = classXML.method.(parameter.length() == 1).@name;
                  key = gameClass[methodName]("msg");
                  if(ExternalInterface.available)
                  {
                     ExternalInterface.call("EncryptionVector",key.toString());
                  }
                  break;
               }
            }
            catch(error:Error)
            {
               continue;
            }
         }
      }
      
      function initTfm(param1:Event = null) : void
      {
         this.tfm.removeEventListener(Event.ADDED_TO_STAGE,this.initTfm);
         this.interfaceHidden = false;
         this.originalAlphaValues = new Dictionary();
         this.gameOutline = new Shape();
         this.gameOutline.graphics.lineStyle(1,0,0.4,true);
         this.gameOutline.graphics.drawRect(-1,22,801,378);
         this.tfm.stage.addEventListener(KeyboardEvent.KEY_UP,this.KeyUp);
      }
      
      function KeyUp(param1:KeyboardEvent) : void
      {
         var _loc2_:int = 0;
         var _loc3_:XML = null;
         var _loc4_:Object = null;
         if(param1.keyCode == 112)
         {
            this.interfaceHidden = !this.interfaceHidden;
            if(!this.interfaceHidden)
            {
               this.tfm.removeChild(this.gameOutline);
            }
            _loc2_ = 0;
            while(_loc2_ < this.tfm.numChildren)
            {
               _loc3_ = describeType(this.tfm.getChildAt(_loc2_));
               if(_loc3_.@name.toString() != "$M")
               {
                  _loc4_ = this.tfm.getChildAt(_loc2_);
                  if(this.interfaceHidden)
                  {
                     this.originalAlphaValues[_loc4_] = _loc4_.alpha;
                     _loc4_.alpha = 0;
                  }
                  else
                  {
                     _loc4_.alpha = this.originalAlphaValues[_loc4_];
                  }
               }
               _loc2_++;
            }
            if(this.interfaceHidden)
            {
               this.tfm.addChild(this.gameOutline);
            }
         }
      }
      
      function SetAlignment(param1:String) : void
      {
         stage.align = param1;
      }
      
      function SetZoom(param1:String) : void
      {
         stage.scaleMode = param1;
      }
      
      function SetQuality(param1:String) : void
      {
         stage.quality = param1;
      }
   }
}
