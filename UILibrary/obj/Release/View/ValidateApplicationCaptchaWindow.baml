   M S B A M L   `   `   `  ����   	UILibrary0    *UILibrary.ValidateApplicationCaptchaWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$   ValidateApplicationCaptcha��$	��200��$	��400�� � 	Resources  �� 
TargetType   HyperlinkText �� Triggers P� Property P� Value �� Property �� Value��%Z   &	        �� " �� P�  �@J�IsMouseOver . . ���  �@�Cursor$
 Hand��.���  � RowDefinitions �� $
��30px ��� $��100px ��� $
��30px �	 � ColumnDefinitions	 �� $��* ��� $��100px �.+��� -   $1��+Please click here to accept the application��$��20(�$��	Underline{�$��2��
����  �#���   �� 
 �� Orientation$
 Vertical=�$��0��$��1��.+��� $K��EThe application requires the code displayed on Twitter validation URL��$
��Wrap=�$��13(�$��5q��� $,��&Click the link above to get this code!��$��5q�$��13(�$��Italic$��� $*��$../Resources/TwitterOfficialIcon.png��$��1��$��1��$��5q��� -    ��Name$ focus��$��20(�$��2������ *	Captcha+ �� Mode$ TwoWay=��� $��Validate��$��1��$��2��$
��5, 0q����� *ValidateCommand+