   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  
MainWindow��5   	   $��1000         ��6   $��1500         ��5	      .��5      � $��0,0,-1024,0q�6    � ColumnDefinitions 5   
   �� 5      �� 5      $	��31* �6   .+�5   
   �� -   5        $��Button��5      $
��Left=�6"   $��108,135,0,0q�6=   $	��Top=�6R   $��1��6   �� 5   
   -   5         ��Name$ MyBox��5      $
��Left=�6!   $��239,148,0,0q�6<   $��TextBox��6Q    �� TextWrapping$
 Wrap=�6`   $	��Top=�6t   $	��455��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   $	��223��6�   $��1��6�   .��6�   z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$ 0.436Q�6&   T� 5      5      .��5   '   5      �� 5   
   -   5        $ InfoBox��5      $��1��6#   $
��Left=�63   $��760,167,0,0q�6N   $��TextBox��6c   $
 Wrap=�6r   $	��Top=�6�   $	��344��6�   $	��179��6�   �� 5   
   -   5        $ ErrorBox��5      $��1��6$   $
��Left=�64   $��494,457,0,0q�6O   $��TextBox��6d   $
 Wrap=�6s   $	��Top=�6�   $	��517��6�   $��87��6�   �� 5   
   -   5        $ 
SlepperBox��5      $��1��6&   $
��Left=�66   $��
28,430,0,0q�6Q   $��TextBox��6e   $
 Wrap=�6t   $	��Top=�6�   $	��198��6�   $��93��6�   �� 5   
   -   5        $ 
WarningBox��5      $��1��6&   $
��Left=�66   $��270,412,0,0q�6Q   $��TextBox��6f   $
 Wrap=�6u   $	��Top=�6�   $	��177��6�   $��88��6�   �� 5   
   -   5        $ ProgressBox��5      $��old progress��6'   $��1��6;   $
��Left=�6K   $��270,606,0,0q�6f   $
 Wrap=�6{   $	��Top=�6�   $	��120��6�   6�   �� 5    
   -   5        $��listBox_detected��5       $��1��6F   $
��Left=�6V   $��
10,606,0,0q�6q   $
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6*   ��  �� Path$ Users�6�   � 5!   
   -	   5         �Name$ 
MyOtherBar��5!      $��1��6*   $
��Left=�6Z   $��33��6u   $��1050,618,0,0q�6�   $	��Top=�6�   $	��251��6�    � Value 6:   �� *ProgressValue+6�   5%      5&      