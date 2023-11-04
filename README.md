# ML_Agents_Examples

###  Installation versions:

C:\ExpML\ML_Agents_Examples>cd venv

C:\ExpML\ML_Agents_Examples\venv>cd Scripts

C:\ExpML\ML_Agents_Examples\venv\Scripts>activate

(venv) C:\ExpML\ML_Agents_Examples\venv\Scripts>pip --version
pip 23.3.1 from C:\ExpML\ML_Agents\venv\lib\site-packages\pip (python 3.10)

(venv) C:\ExpML\ML_Agents_Examples\venv\Scripts>pip3 --version
pip 23.3.1 from C:\ExpML\ML_Agents\venv\lib\site-packages\pip (python 3.10)


---------------

# Windows Installation Steps in short
---------------

### Branch currently being followed is : release 21


Install Anaconda

1. In Anaconda, executed this command:

conda create -n mlagents python=3.10.12 && conda activate mlagents

Then everytime upon opening Anaconda, switch to the ML-Agents environment fromn the top ribbon.
or by entering this in CMD:
conda activate mlagents


2. Then launch CMD[install if needed]

(mlagents) C:\ExpML\ML_Agents_Examples>python -m venv venv

(mlagents) C:\ExpML\ML_Agents_Examples>cd venv\Scripts

(mlagents) C:\ExpML\ML_Agents_Examples\venv\Scripts>activate

(venv) (mlagents) C:\ExpML\ML_Agents_Examples\venv\Scripts>python --version
Python 3.10.13

(venv) (mlagents) C:\ExpML\ML_Agents_Examples\venv\Scripts>python -m pip install --upgrade pip
Requirement already satisfied: pip in c:\expml\ml_agents_examples\venv\lib\site-packages (23.0.1)
Collecting pip
  Using cached pip-23.3.1-py3-none-any.whl (2.1 MB)
Installing collected packages: pip
  Attempting uninstall: pip
    Found existing installation: pip 23.0.1
    Uninstalling pip-23.0.1:
      Successfully uninstalled pip-23.0.1
Successfully installed pip-23.3.1

(venv) (mlagents) C:\ExpML\ML_Agents_Examples\venv\Scripts>pip --version
pip 23.3.1 from C:\ExpML\ML_Agents_Examples\venv\lib\site-packages\pip (python 3.10)

### Installing pytorch :

(venv) (mlagents) C:\ExpML\ML_Agents_Examples\venv\Scripts>pip3 install torch~=1.13.1 -f https://download.pytorch.org/whl/torch_stable.html
Looking in links: https://download.pytorch.org/whl/torch_stable.html
Collecting torch~=1.13.1
  Downloading https://download.pytorch.org/whl/cu117/torch-1.13.1%2Bcu117-cp310-cp310-win_amd64.whl (2255.4 MB)
     ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ 2.3/2.3 GB 3.5 MB/s eta 0:00:00
Collecting typing-extensions (from torch~=1.13.1)
  Downloading typing_extensions-4.8.0-py3-none-any.whl.metadata (3.0 kB)
Downloading typing_extensions-4.8.0-py3-none-any.whl (31 kB)
Installing collected packages: typing-extensions, torch
Successfully installed torch-1.13.1+cu117 typing-extensions-4.8.0

You can check the installation details for PyTorch:

(venv) (mlagents) C:\ExpML\ML_Agents_Examples>pip show torch
Name: torch
Version: 1.13.1+cu117
Summary: Tensors and Dynamic neural networks in Python with strong GPU acceleration
Home-page: https://pytorch.org/
Author: PyTorch Team
Author-email: packages@pytorch.org
License: BSD-3
Location: c:\expml\ml_agents_examples\venv\lib\site-packages
Requires: typing-extensions
Required-by: mlagents

----
As you can see, it has cu117, this means we need to install CUDA 11.7

CUDA 11.7


CUDA is necessary if you have a good supporting graphics card. It will delegate some work-load to graphics card instead of CPU, thus quick training.
Since we have an isolated environment, we can install CUDA in this isolated environment.
Here is the command:

conda install -c nvidia/label/cuda-11.7.0 cuda-toolkit

To check if installation was successful :
(mlagents) (venv) C:\ExpML\ML_Agents_Examples\venv\Scripts>nvcc --version
nvcc: NVIDIA (R) Cuda compiler driver
Copyright (c) 2005-2022 NVIDIA Corporation
Built on Tue_May__3_19:00:59_Pacific_Daylight_Time_2022
Cuda compilation tools, release 11.7, V11.7.64
Build cuda_11.7.r11.7/compiler.31294372_0

----
Ok, later found that we also need cuDNN isntallation.

According to this link
https://developer.nvidia.com/rdp/cudnn-archive

We have to intall cuDNN version 8.1.0.

Command for that:

conda install -c conda-forge cudnn=8.1.0

It will ask for a confirmation, say Yes[Press Y].

Once done with installation, verify using this:
(mlagents) (venv) C:\ExpML\ML_Agents_Examples\venv\Scripts>conda list cudnn
 packages in environment at C:\Users\kisho\anaconda3\envs\mlagents:

Name                    Version                   Build  Channel
cudnn                     8.1.0.77             h3e0f4f4_0    conda-forge

### Installing ML Agents [FAILED]:

(venv) (mlagents) C:\ExpML\ML_Agents_Examples\venv\Scripts>pip install mlagents
Collecting mlagents
  Downloading mlagents-0.28.0-py3-none-any.whl (164 kB)
     ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ 164.6/164.6 kB 1.2 MB/s eta 0:00:00
Collecting grpcio>=1.11.0 (from mlagents)
  Downloading grpcio-1.59.2-cp310-cp310-win_amd64.whl.metadata (4.2 kB)
Collecting h5py>=2.9.0 (from mlagents)

.
.

.
   ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ 124.2/124.2 kB 7.1 MB/s eta 0:00:00
Installing collected packages: pywin32, urllib3, tensorboard-data-server, six, pyyaml, pypiwin32, pyasn1, protobuf, Pillow, oauthlib, numpy, MarkupSafe, markdown, idna, grpcio, cloudpickle, charset-normalizer, certifi, cachetools, attrs, absl-py, werkzeug, rsa, requests, pyasn1-modules, mlagents-envs, h5py, cattrs, requests-oauthlib, google-auth, google-auth-oauthlib, tensorboard, mlagents
Successfully installed MarkupSafe-2.1.3 Pillow-10.1.0 absl-py-2.0.0 attrs-23.1.0 cachetools-5.3.2 cattrs-1.5.0 certifi-2023.7.22 charset-normalizer-3.3.2 cloudpickle-3.0.0 google-auth-2.23.4 google-auth-oauthlib-1.1.0 grpcio-1.59.2 h5py-3.10.0 idna-3.4 markdown-3.5.1 mlagents-0.28.0 mlagents-envs-0.28.0 numpy-1.26.1 oauthlib-3.2.2 protobuf-4.23.4 pyasn1-0.5.0 pyasn1-modules-0.3.0 pypiwin32-223 pywin32-306 pyyaml-6.0.1 requests-2.31.0 requests-oauthlib-1.3.1 rsa-4.9 six-1.16.0 tensorboard-2.15.1 tensorboard-data-server-0.7.2 urllib3-2.0.7 werkzeug-3.0.1

### Installing ML Agents:
Git clone the repo and switch to release 21, in a different place. 
Copy the folders : 
../ml-agents/ml-agents 
../ml-agents/ml-agents-envs

Execute the following commands in CMD to install ML Agents:

# ML Agents envs installation
python -m pip install ./ml-agents-envs

ERROR: Failed building wheel for numpy

Just upgrade VS tools from here:
https://visualstudio.microsoft.com/downloads/

Installations done for this workspace:

>   https://aka.ms/vs/17/release/VC_redist.x64.exe


Build Tools for Visual Studio 2022

These Build Tools allow you to build Visual Studio projects from a command-line interface. Supported projects include: ASP.NET, Azure, C++ desktop, ClickOnce, containers, .NET Core, .NET Desktop, Node.js, Office and SharePoint, Python, TypeScript, Unit Tests, UWP, WCF, and Xamarin. Use of this tool requires a valid Visual Studio license, unless you are building open-source dependencies for your project. See the Build Tools license for more details.

>    https://aka.ms/vs/17/release/vs_BuildTools.exe

RESTART your machine.



--------------------


ERROR:

  note: This error originates from a subprocess, and is likely not a problem with pip.
  ERROR: Failed building wheel for numpy
Successfully built mlagents-envs
Failed to build numpy
ERROR: Could not build wheels for numpy, which is required to install pyproject.toml-based projects

Solution

(venv) (mlagents) C:\ExpML\ML_Agents_Examples>pip install numpy

--------------------


ERROR: 

 note: This error originates from a subprocess, and is likely not a problem with pip.
  ERROR: Failed building wheel for numpy
Successfully built mlagents-envs
Failed to build numpy
ERROR: Could not build wheels for numpy, which is required to install pyproject.toml-based projects

Solution:

pip install setuptools==49.1.2

--------------------

Final command:

python -m pip install ./ml-agents-envs

--------------------


# ML Agents installation

python -m pip install ./ml-agents

--------------------

Had no issues with this install.

Test : 

mlagents-learn --help


NOTE:
Since we are installing these from a folder, don't worry about which version got installed.

We are making this work for release 21 branch.

### Examples: 

Branch currently being followed is : release 21

Helpful links:

https://unity-technologies.github.io/ml-agents/Installation/
https://www.youtube.com/watch?v=zPFU30tbyKs&ab_channel=CodeMonkey

Various project tutorials once the setup is in place.

https://huggingface.co/learn/deep-rl-course/unit5/introduction

https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Learning-Environment-Examples.md

Installation for Windows:

https://github.com/Unity-Technologies/ml-agents/blob/release-21-branch/docs/Installation-Anaconda-Windows.md
