   M S B A M L   `   `   `  ����  	UILibrary0    *UILibrary.ValidateApplicationCaptchaWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$   ValidateApplicationCaptcha��5   	   $	��200��6,   $	��400��69    � 	Resources 5       �� 
TargetType   HyperlinkText �� Triggers P� Property P� Value �� Property �� Value��%�   &	        �� 5   
   " ��6    5      P� 5       �@J�IsMouseOver . . �5	      ��  �@�Cursor$
 Hand��6/   5
      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��30px �6   �� 5      $��100px �6   �� 5      $
��30px �6   	 � ColumnDefinitions	 5   
   �� 5      $��* �6   �� 5      $��100px �6   .+�5   
   �� -   5        $1��+Please click here to accept the application��5      $��20(�5      $��	Underline{�6"   $��2��5      
����  �#���   5      �� 5"   
   
 �� Orientation$
 Vertical=�6   $��0��5#      $��1��6%   .+�5$      �� $K��EThe application requires the code displayed on Twitter validation URL��6   $
��Wrap=�5%      $��13(�6,   $��5q�5&      �� 5'      $,��&Click the link above to get this code!��6   $��5q�5(      $��13(�6#   $��Italic$�61   5)      �� 5,   
   $*��$../Resources/TwitterOfficialIcon.png��6   $��1��5-      $��1��6    $��5q�5.      �� 51   
   -   5         ��Name$ focus��51      $��20(�52      $��2��53      ��51   !   �� *	Captcha+ �� Mode$ TwoWay=�54   ,   �� 56   
   $��Validate��6   $��1��58      $��2��6!   $
��5, 0q�59      ��57      �� *ValidateCommand+59      5:      5;      