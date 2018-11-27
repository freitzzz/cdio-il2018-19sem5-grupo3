% guillotine_packaging(+Packages,+Container,-L)
% Packages => List with the packages being organized (CurrentPackage|RemainingPackages)
% Container => Container where the packages will be packed
% L => Tuple List with the organized packages by level??

guillotine_packaging([CP|RP],Container,MC):-