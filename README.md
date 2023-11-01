# ML_Agents_Examples

###  Installation versions:

C:\ExpML\ML_Agents_Examples>cd venv

C:\ExpML\ML_Agents_Examples\venv>cd Scripts

C:\ExpML\ML_Agents_Examples\venv\Scripts>activate

(venv) C:\ExpML\ML_Agents_Examples\venv\Scripts>pip --version
pip 23.3.1 from C:\ExpML\ML_Agents\venv\lib\site-packages\pip (python 3.10)

(venv) C:\ExpML\ML_Agents_Examples\venv\Scripts>pip3 --version
pip 23.3.1 from C:\ExpML\ML_Agents\venv\lib\site-packages\pip (python 3.10)



### Install the `com.unity.ml-agents` Unity package

The Unity ML-Agents C# SDK is a Unity Package. You can install the
`com.unity.ml-agents` package
[directly from the Package Manager registry](https://docs.unity3d.com/Manual/upm-ui-install.html).
Please make sure you enable 'Preview Packages' in the 'Advanced' dropdown in
order to find it.

**NOTE:** If you do not see the ML-Agents package listed in the Package Manager
please follow the [advanced installation instructions](#advanced-local-installation-for-development) below.

#### Advanced: Local Installation for Development

You can [add the local](https://docs.unity3d.com/Manual/upm-ui-local.html)
`com.unity.ml-agents` package (from the repository that you just cloned) to your
project by:

1. navigating to the menu `Window` -> `Package Manager`.
1. In the package manager window click on the `+` button.
1. Select `Add package from disk...`
1. Navigate into the `com.unity.ml-agents` folder.
1. Select the `package.json` file.

**NOTE:** In Unity 2018.4 the `+` button is on the bottom right of the packages
list, and in Unity 2019.3 it's on the top left of the packages list.

<p align="center">
  <img src="images/unity_package_manager_window.png"
       alt="Unity Package Manager Window"
       height="300"
       border="10" />
  <img src="images/unity_package_json.png"
     alt="package.json"
     height="300"
     border="10" />
</p>

If you are going to follow the examples from our documentation, you can open the
`Project` folder in Unity and start tinkering immediately.

### Install the `mlagents` Python package

Installing the `mlagents` Python package involves installing other Python
packages that `mlagents` depends on. So you may run into installation issues if
your machine has older versions of any of those dependencies already installed.
Consequently, our supported path for installing `mlagents` is to leverage Python
Virtual Environments. Virtual Environments provide a mechanism for isolating the
dependencies for each project and are supported on Mac / Windows / Linux. We
offer a dedicated [guide on Virtual Environments](Using-Virtual-Environment.md).

#### (Windows) Installing PyTorch

On Windows, you'll have to install the PyTorch package separately prior to
installing ML-Agents. Activate your virtual environment and run from the command line:

```sh
pip3 install torch==1.7.0 -f https://download.pytorch.org/whl/torch_stable.html
```

Note that on Windows, you may also need Microsoft's
[Visual C++ Redistributable](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)
if you don't have it already. See the [PyTorch installation guide](https://pytorch.org/get-started/locally/)
for more installation options and versions.

#### Installing `mlagents`

To install the `mlagents` Python package, activate your virtual environment and
run from the command line:

```sh
pip3 install mlagents
```

Note that this will install `mlagents` from PyPi, _not_ from the cloned
repository. If you installed this correctly, you should be able to run
`mlagents-learn --help`, after which you will see the command
line parameters you can use with `mlagents-learn`.

By installing the `mlagents` package, the dependencies listed in the
[setup.py file](../ml-agents/setup.py) are also installed. These include
[PyTorch](Background-PyTorch.md) (Requires a CPU w/ AVX support).

#### Advanced: Local Installation for Development

If you intend to make modifications to `mlagents` or `mlagents_envs`, you should
install the packages from the cloned repository rather than from PyPi. To do
this, you will need to install `mlagents` and `mlagents_envs` separately. From
the repository's root directory, run:

```sh
pip3 install torch -f https://download.pytorch.org/whl/torch_stable.html
pip3 install -e ./ml-agents-envs
pip3 install -e ./ml-agents
```

Running pip with the `-e` flag will let you make changes to the Python files
directly and have those reflected when you run `mlagents-learn`. It is important
to install these packages in this order as the `mlagents` package depends on
`mlagents_envs`, and installing it in the other order will download
`mlagents_envs` from PyPi.