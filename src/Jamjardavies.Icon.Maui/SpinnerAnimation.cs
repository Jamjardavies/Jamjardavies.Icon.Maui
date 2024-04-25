// <copyright file="SpinnerAnimation.cs" company="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui;

internal class SpinnerAnimation : Animation
{
    private const string SpinnerAnimationName = "SpinnerAnimation";

    #region Fields

    private readonly VisualElement element;

    #endregion

    public SpinnerAnimation(VisualElement element)
        : base(v => element.Rotation = v, 0, 360)
    {
        this.element = element;
    }

    public double Length { get; set; } = 1;

    #region Methods

    #region Public

    public void Start()
    {
        this.Commit(
            this.element,
            SpinnerAnimationName,
            16,
            (uint)(this.Length * 1000),
            this.Easing,
            (_, _) => this.element.Rotation = 0,
            () => true);
    }

    public void Stop()
    {
        this.element.AbortAnimation(SpinnerAnimationName);
    }

    #endregion

    #endregion
}