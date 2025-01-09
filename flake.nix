{
  description = "fs flake";

  inputs.nixpkgs.url = "nixpkgs/nixpkgs-unstable";
  inputs.flake-parts.url = "github:hercules-ci/flake-parts";

  outputs =
    inputs@{
      self,
      nixpkgs,
      flake-parts,
      ...
    }:
    flake-parts.lib.mkFlake { inherit inputs; } {
      systems = [
        "x86_64-linux"
        "aarch64-linux"
        "aarch64-darwin"
        "x86_64-darwin"
      ];

      perSystem =
        { pkgs, ... }:
        {
          devShells = {
            default = pkgs.mkShell {
              name = "dev-shell";
              buildInputs = with pkgs; [
                dotnet-sdk_9
              ];
            };
          };

          # Add command-line executables
          packages = {
            default = pkgs.writeShellScriptBin "default" ''
              if [ -z "$1" ]; then
                echo "Usage: default-icon <name>"
                exit 1
              fi
              ${pkgs.dotnet-sdk_9}/bin/dotnet run --project src/IconDownloader --name "$1"
            '';

            bold = pkgs.writeShellScriptBin "bold" ''
              if [ -z "$1" ]; then
                echo "Usage: bold-icon <name>"
                exit 1
              fi
              ${pkgs.dotnet-sdk_9}/bin/dotnet run --project src/IconDownloader --name "$1" --style bold
            '';

            filled = pkgs.writeShellScriptBin "filled" ''
              if [ -z "$1" ]; then
                echo "Usage: filled-icon <name>"
                exit 1
              fi
              ${pkgs.dotnet-sdk_9}/bin/dotnet run --project src/IconDownloader --name "$1" --style filled
            '';
          };
        };
    };
}
