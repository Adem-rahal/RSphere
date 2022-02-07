# RSphere
This is a quantum visualisation tool that help you get a grasp of quantum states with a more intuitive approach. Get more inormation here: [PROJECT PAGE](https://dvic.devinci.fr/project/RSphere)

 [![R Sphere video](https://img.youtube.com/vi/BST--Oy6bvo/0.jpg)](https://www.youtube.com/watch?v=BST--Oy6bvo) 
 
**VIDEO**

# Installation

To tun the program click [HERE](https://drive.google.com/drive/folders/171hFvPjeD_OJeHCSoSJDhymYpGQsjr3h?usp=sharing). 
Then download the appropriate version for your OS, for Windows you have to run the .exe file and for Linux you will have to run the .x86_64 file.
Mac version could be added quickly if needed just send me an e-mail at adem.rahal@edu.devinci.fr

# Indication on the software

Their are a few things you need to know about quantum computing to run the software, first the qubit starts on the |0> the only way to change its state is to apply quantum gates to it. 

Like in classical computing you have gates to change the state of a qubit. For the different gates implemented you have the Hadamard gate (H-gate) that puts the qubit in a perfect superposition, Then you have the S-Gate changes the phase of the qubit, the RZ-Gate or rotation-z gate that rotate the qubit on the Z axis, the Pauli gates, and finnally you have customable gates that you can change yourself by adding the proper matrix.

When you clic on the measurement button the qubit fonction wave will collapse to choose a value either 1 or 0 according to his state, if it is in perfect supperposition you will have the same chance to get the |0> state and the |1> state, etc. The more the cirlce is full the more chance you have to get the |1> state on measurement.

This is a very simple implementation more gates and options will be added soon.

# Gates customisation

To add you own custom gates you can go the Asset/Scripts/QubitState.cs script and scroll down to the gate section part. To add your gate you should make sure first that it is unitary then you can fill up the gate_matrix[] variable with your own values make sure to use the Complex class to do so some examples are already implemented. The default gates represents the Identity matrix.

Then you can rebuild the Unity project on Unity with all the files (an easier way to do this might be implemented in the futur).
