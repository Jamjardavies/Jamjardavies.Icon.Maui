﻿// <copyright file="ISpinnable.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui;

internal interface ISpinnable
{
    #region Properties

    /// <summary>
    ///     Is the icon spinning?
    /// </summary>
    bool Spin { get; set; }

    /// <summary>
    ///     Sets how long it takes the icon to spin, in seconds.
    /// </summary>
    double SpinDuration { get; set; }

    #endregion

    #region Methods

    #region Public

    /// <summary>
    ///     Starts the spinning of the Icon.
    /// </summary>
    void BeginSpin();

    /// <summary>
    ///     Stops the spinning of the Icon.
    /// </summary>
    void StopSpin();

    #endregion

    #endregion
}