   M S B A M L   `   `   `  ���� G  CImageEffects, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ImageEffectsImageEffects      ImageEffects.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:ImageEffects       � Title$  Image Effects��$	��607��$	��951��.��� -    �Name$ MainGrid��$��0,0,0,1q�.+��� -    ��Name$ 	ImgOutput��$��345,12,10,11q��� -    ��Name$ BtnSelectFile��$��Select Image��$
��Left=�$��8,10,0,0q�$	��Top=�$	��242���� -    ��Name$ 
LblEffects��$��Effects:��$
��Left=�$��8,54,0,0q�$	��Top=�	� -    	�Name$ ChkBoxDownscale�� 	� 	GroupName$ Effect��$��	Downscale��$
��Left=�$��	10,80,0,0q�$	��Top=� J� RenderTransformOrigin$ 0.588,0.533&�{�.  �� -   $ BtnRun��$��Apply Effects��$
��Left=�$��
29,535,0,0q�$ 0.5,0.5&�$	��108��$��20��$	��Top=�{�.  z�V� .~��� �� ��  �� Angle$ 0.535Q�T� �� -   $ LblSelectedFile��$�� ��$
��Left=�$��	10,30,0,0q�$	��Top=�$	��240���� -   	 ��Name$	 TxtBoxDownscaleNum��$
��Left=�$��
256,78,0,0q�
 �� TextWrapping$

 Wrap=�$��1��$	��Top=�$��28���� $��Resolution:��$
��Left=�$��
188,75,0,0q�$	��Top=�	� -	   $ ChkBoxPixelSort��$ Effect��$��
Pixel Sort��$
��Left=�$��
10,122,0,0q�$	��Top=�$ 0.588,0.533&�	� -
   $ RadBtnVertical��$��Vertical��$ PixelSortType��$
��Left=�$��122,112,0,0q�$	��Top=� d� 	IsChecked$
 TrueY�	� -   $ RadBtnHorizontal��$��
Horizontal��$ PixelSortType��$
��Left=�$��122,127,0,0q�$	��Top=��� -   $ LblRowsOrCols��$��Rows:��$
��Left=�$��216,111,0,0q�$	��Top=��� -   $	 TxtBoxPixelSortCols��$
��Left=�$��256,115,0,0q�$

 Wrap=�$��1��$	��Top=�$��28���� -   $ BtnSave��$��
Save Image��$
��Left=�$��150,535,0,0q�$ 0.5,0.5&�$	��108��{�.  $��20��$	��Top=�z�V� .~��� �� �� $ 0.535Q�T� 	� -   $ RadBtnBitShift��$ Effect��$��	Bit Shift��$
��Left=�$��
10,172,0,0q�$	��Top=�$ 0.588,0.533&�	� -   $ 
RadBtnLeft��$��
Left Shift��$ ShiftDirection��$
��Left=�$��122,163,0,0q�$	��Top=�$
 TrueY�	� -   $ RadBtnRight��$��Right Shift��$ ShiftDirection��$
��Left=�$��122,178,0,0q�$	��Top=��� -   $ LblBits��$��Bits:��$
��Left=�$��221,165,0,0q�$	��Top=��� -   $	 
TxtBoxBits��$
��Left=�$��256,169,0,0q�$

 Wrap=�$��1��$	��Top=�$��28��	� -   $ RadBtnBitWise��$ Effect��$��Bitwise��$
��Left=�$��
10,235,0,0q�$	��Top=�$ 0.588,0.533&��� -    ��Name$ ComboColors��$
��Left=�$��188,242,0,0q�$	��Top=�$��96���� -   $ LblRowsOrCols_Copy��$��Compare to...��$
��Left=�$��204,219,0,0q�$	��Top=�$��80���� -   $ BtnReset��$��Reset��$
��Left=�$��
29,509,0,0q�$	��108��{�.  $��20��$	��Top=�	� -   $ RadBtnBitWiseVariants��$ Effect��$��Bitwise Variants��$
��Left=�$��
10,322,0,0q�$	��Top=�$ 0.588,0.533&�$	��101��$��18��	� -   $ RadBtnOr��$��OR��$ BitwiseOperation��$
��Left=�$��122,230,0,0q�$	��Top=�$
 TrueY�	� -   $ 	RadBtnAnd��$	��AND��$ BitwiseOperation��$
��Left=�$��122,246,0,0q�$	��Top=�	� -   $ 	RadBtnXor��$	��XOR��$ BitwiseOperation��$
��Left=�$��122,260,0,0q�$	��Top=�$
 TrueY��� -   $ ComboColorsVariant��$
��Left=�$��188,332,0,0q�$	��Top=�$��96���� -   $ LblRowsOrCols_Copy1��$��Compare to...��$
��Left=�$��204,309,0,0q�$	��Top=�$��80��	� -   $ RadBtnOrVariant��$��OR��$ BitwiseOperationVariant��$
��Left=�$��122,321,0,0q�$	��Top=�$
 TrueY�	� -   $ RadBtnAndVariant��$	��AND��$ BitwiseOperationVariant��$
��Left=�$��122,336,0,0q�$	��Top=�	� -    $ RadBtnXorVariant��$	��XOR��$ BitwiseOperationVariant��$
��Left=�$��122,351,0,0q�$	��Top=�$
 TrueY��� -!   $ BtnSaveVariants��$��Save Variants��$
��Left=�$��150,510,0,0q�$ 0.5,0.5&�$	��108��$��20��$	��Top=�{�.  z�V� .~��� �� �� $ 0.535Q�T� �� -"   $ LblBits_Copy��$��Variant:��$
��Left=�$��
29,338,0,0q�$	��Top=��� -#   $	 TxtBoxVariant��$
��Left=�$��
78,344,0,0q�$

 Wrap=�$��1��$	��Top=�$��22��$ 0.273,-0.109&�