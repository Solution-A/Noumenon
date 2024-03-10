using Dalamud.Plugin;
using Penumbra.Api.Enums;
using Penumbra.Api.Helpers;

namespace Penumbra.Api;

public static partial class Ipc
{
    /// <inheritdoc cref="IPenumbraApi.GetDrawObjectInfo"/>
    public static class GetDrawObjectInfo
    {
        public const string Label = $"Penumbra.{nameof(GetDrawObjectInfo)}";

        public static FuncProvider<nint, (nint, string)> Provider(DalamudPluginInterface pi, Func<nint, (nint, string)> func)
            => new(pi, Label, func);

        public static FuncSubscriber<nint, (nint, string)> Subscriber(DalamudPluginInterface pi)
            => new(pi, Label);
    }

    /// <inheritdoc cref="IPenumbraApi.GetCutsceneParentIndex"/>
    public static class GetCutsceneParentIndex
    {
        public const string Label = $"Penumbra.{nameof(GetCutsceneParentIndex)}";

        public static FuncProvider<int, int> Provider(DalamudPluginInterface pi, Func<int, int> func)
            => new(pi, Label, func);

        public static FuncSubscriber<int, int> Subscriber(DalamudPluginInterface pi)
            => new(pi, Label);
    }

    /// <inheritdoc cref="IPenumbraApi.SetCutsceneParentIndex"/>
    public static class SetCutsceneParentIndex
    {
        public const string Label = $"Penumbra.{nameof(SetCutsceneParentIndex)}";

        public static FuncProvider<int, int, PenumbraApiEc> Provider(DalamudPluginInterface pi, Func<int, int, PenumbraApiEc> func)
            => new(pi, Label, func);

        public static FuncSubscriber<int, int, PenumbraApiEc> Subscriber(DalamudPluginInterface pi)
            => new(pi, Label);
    }

    /// <inheritdoc cref="IPenumbraApi.CreatingCharacterBase"/>
    public static class CreatingCharacterBase
    {
        public const string Label = $"Penumbra.{nameof(CreatingCharacterBase)}";

        public static EventProvider<nint, string, nint, nint, nint> Provider(DalamudPluginInterface pi, Action add, Action del)
            => new(pi, Label, add, del);

        public static EventSubscriber<nint, string, nint, nint, nint> Subscriber(DalamudPluginInterface pi,
            params Action<nint, string, nint, nint, nint>[] actions)
            => new(pi, Label, actions);
    }

    /// <inheritdoc cref="IPenumbraApi.CreatedCharacterBase"/>
    public static class CreatedCharacterBase
    {
        public const string Label = $"Penumbra.{nameof(CreatedCharacterBase)}";

        public static EventProvider<nint, string, nint> Provider(DalamudPluginInterface pi, Action add, Action del)
            => new(pi, Label, add, del);

        public static EventSubscriber<nint, string, nint> Subscriber(DalamudPluginInterface pi, params Action<nint, string, nint>[] actions)
            => new(pi, Label, actions);
    }

    /// <inheritdoc cref="IPenumbraApi.GameObjectResourceResolved"/>
    public static class GameObjectResourcePathResolved
    {
        public const string Label = $"Penumbra.{nameof(GameObjectResourcePathResolved)}";

        public static EventProvider<nint, string, string> Provider(DalamudPluginInterface pi, Action add, Action del)
            => new(pi, Label, add, del);

        public static EventSubscriber<nint, string, string> Subscriber(DalamudPluginInterface pi, params Action<nint, string, string>[] actions)
            => new(pi, Label, actions);
    }
}
