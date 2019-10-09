# Why separate folder?

Docker tooling in Visual Studio has an issue with supplementary Dockerfile projects and docker-compose. The build error is `CTC1030	Value cannot be null. Parameter name: stream`.

For that reason, we need to have Dockerfile in a subfolder.

You can track the issue here: https://github.com/Microsoft/DockerTools/issues/141

